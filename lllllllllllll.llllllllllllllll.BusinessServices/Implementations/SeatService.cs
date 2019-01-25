using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using BestTime.Data.Common;
using *********.**********.BusinessServices.Enums;
using *********.**********.BusinessServices.Interfaces;
using *********.**********.BusinessServices.Models;
using *********.**********.BusinessServices.Repositories;

namespace *********.**********.BusinessServices.Implementations
{
    public class SeatService : ISeatService
    {
        private readonly IMongoRepository _repository;

        public SeatService(IMongoRepository repository)
        {
            _repository = repository;
        }

        public bool MapHasSeats(string html)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(html);
            var seatsHtml = document.QuerySelectorAll(".zoomer .map__svg .seats").FirstOrDefault();
            return seatsHtml != null && seatsHtml.Children.Any();
        }

        public bool MapHasStandingSeats(string html)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(html);
            var standingSeatsHtml = document.QuerySelectorAll(".zoomer .map__svg .polygons").FirstOrDefault();
            return standingSeatsHtml != null && standingSeatsHtml.Children.Any(x => x.ClassList.Any(z => z == "is-ga"));
        }

        public async Task SaveAvailableSeats(List<Facet> facets, List<Seat> seats, List<Offer> offers, string bteId, string url)
        {
            foreach (var facet in facets)
            {
                var offer = offers.Where(x => facet.Offers.Any(z => z == x.OfferId)).ToList();
                foreach (var placeId in facet.Places)
                {
                    var place = seats.Where(x => x.Id == placeId).ToList();
                    foreach (var seat in place)
                    {
                        SetSeatInformation(seat, offer, facet);
                    }
                }
                if (facet.Available)
                {
                    foreach (var shape in facet.Shapes)
                    {
                        var standingSeats = seats.Where(x => x.IsStanding && x.Id == shape);
                        foreach (var seat in standingSeats)
                        {
                            SetSeatInformation(seat, offer, facet);
                        }

                    }
                }

            }

            var groups = seats.Where(x => !x.IsStanding).GroupBy(x => new { x.Section, x.Row }).ToList();
            var ticketGroups = new List<List<Seat>>();

            foreach (var group in groups)
            {
                var priceLevelId = group.FirstOrDefault()?.Offer?.PriceLevelId;
                var ticketGroup = new List<Seat>();
                Seat previousSeat = null;

                foreach (var seat in group)
                {
                    if (IsNextInGroup(seat, previousSeat, priceLevelId))
                    {
                        if (ticketGroup.Any())
                        {
                            ticketGroups.Add(ticketGroup);
                            ticketGroup = new List<Seat>();
                        }
                        priceLevelId = seat.Offer?.PriceLevelId;
                    }
                    if (seat.IsAvailable)
                    {
                        ticketGroup.Add(seat);
                        previousSeat = seat;
                    }
                }
                if (ticketGroup.Any())
                {
                    ticketGroups.Add(ticketGroup);
                }

            }

            var ticketsNumber = seats.Count(x => x.IsAvailable && !x.IsStanding);

            var standingGroups = seats.Where(x => x.IsStanding).ToList();
            foreach (var group in standingGroups)
            {
                ticketGroups.Add(new List<Seat>() { group });
            }

           // await SaveTicketGroups(ticketGroups, bteId, ticketsNumber, url);
        }

        private bool IsNextInGroup(Seat currentSeat, Seat previousSeat, int? priceLevelId)
        {
            int currentSeatNumber;
            bool isNumericСurrentSeat = int.TryParse(currentSeat?.Name, out currentSeatNumber);
            int previousSeatNumber;
            bool isNumericPreviousSeat = int.TryParse(previousSeat?.Name, out previousSeatNumber);

            var matchAccessibility = true;

            if (previousSeat != null && currentSeat != null && (currentSeat.Accessibility.Any() || previousSeat.Accessibility.Any()))
            {

                var accessibility = new List<string>();
                accessibility.AddRange(currentSeat.Accessibility);
                accessibility.AddRange(previousSeat.Accessibility);
                accessibility = accessibility.Distinct().ToList();

                matchAccessibility = accessibility.OrderBy(x => x).SequenceEqual(new List<string> { "companion", "wheelchair" }) || currentSeat.Accessibility.SequenceEqual(previousSeat.Accessibility);
            }

            return currentSeat?.Offer?.PriceLevelId != priceLevelId || !matchAccessibility
                   || (isNumericСurrentSeat && isNumericPreviousSeat && (Math.Abs(previousSeatNumber - currentSeatNumber) > 2 || previousSeat?.Description != currentSeat?.Description));

        }


        private async Task SaveTicketGroups(List<List<Seat>> groups, int bteId, int ticketsNumber, string url)
        {
            var ticketGroups = new List<TicketGroup>();

            foreach (var group in groups)
            {
                var accessibility = new List<string>();
                var inventoryTypes = new List<string>();
                foreach (var seat in group)
                {
                    accessibility.AddRange(seat.Accessibility);
                    inventoryTypes.AddRange(seat.InventoryTypes);
                }

                accessibility = accessibility.Distinct().OrderBy(x => x).ToList();
                inventoryTypes = inventoryTypes.Distinct().OrderBy(x => x).ToList();

                var firstSeat = group.First();

                var ticketGroup = new TicketGroup()
                {
                    //BteId = bteId,
                    SeatQty = group.Count,
                    ObstructedView = false,
                    StartSeatNumber = firstSeat.Name,
                    EndSeatNumber = group.Last().Name,
                    Row = firstSeat.Row,
                    Section = firstSeat.Section,
                    Offers = firstSeat.Offers.Distinct().ToList(),
                    SeatTypes = firstSeat.SeatTypes,
                    Accessibility = accessibility,
                    InventoryTypes = inventoryTypes,

                };
                ticketGroups.Add(ticketGroup);
            }

            var model = new TicketGroupModel
            {
                Groups = ticketGroups,
                DatePulled = DateTime.UtcNow,
                //BteId = bteId,
                TicketsNumber = ticketsNumber,
                Result = SiteResponse.Success,
                Url = url
            };
            
            await _repository.Add(model);

        }

        private void SetSeatInformation(Seat seat, List<Offer> offers, Facet facet)
        {
            seat.Offers.AddRange(offers);
            seat.OfferTypes.AddRange(facet.OfferTypes);
            seat.IsAvailable = facet.Available;
            seat.Description = facet.Description;
            seat.Accessibility.AddRange(facet.Accessibility);
            seat.InventoryTypes.AddRange(facet.InventoryTypes);
        }
    }
}
