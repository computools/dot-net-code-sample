using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using BestTime.Data.Common;
using *********.**********.BusinessServices.Enums;
using *********.**********.BusinessServices.Interfaces;
using *********.**********.BusinessServices.Models;
using *********.**********.BusinessServices.Repositories;
using Newtonsoft.Json;
using System.Threading;

namespace *********.**********.BusinessServices.Implementations
{
    public class HtmlParseService : IHtmlParseService
    {
        public Tuple<string, string> ParseApiData(string html)
        {
            var apikey = Regex.Match(html, @"apikey: '.*'", RegexOptions.IgnoreCase).Value.Split('\'')[1];
            var apiSecret = Regex.Match(html, @"apiSecret: '.*'", RegexOptions.IgnoreCase).Value.Split('\'')[1];
            return new Tuple<string, string>(apikey, apiSecret);
        }

        /// <summary>
        /// Parse the Offer Json into List of Offers
        /// </summary>
        /// <param name="htmlTask"></param>
        /// <returns></returns>
        public async Task<List<Offer>> ParseOffersInfoJson(string htmlTask)
        {
            await Task.Delay(1);
            List<Offer> lstOffer = new List<Offer>();

            //var html = htmlTask.Result;
            var html = htmlTask;

            if (!string.IsNullOrEmpty(html))
            {

                var offersString = Regex.Match(html, @"\{.*\}", RegexOptions.IgnoreCase).Value;
                OfferModal quickPic = JsonConvert.DeserializeObject<OfferModal>(offersString);



                if (quickPic != null && quickPic._embedded != null && quickPic._embedded.item != null)
                {
                    lstOffer = (from offer in quickPic._embedded.item
                                select new Offer()
                                {
                                    OfferId = offer.offerId,
                                    Name = offer.name,
                                    ListPrice = offer.listPrice == null ? "0.0" : offer.listPrice,
                                    TotalPrice = offer.totalPrice.HasValue ? offer.totalPrice.Value : 0.0m,
                                    FaceValue = offer.faceValue == null ? "0.0" : offer.faceValue,
                                    Currency = offer.currency,
                                    PriceLevelId = offer.priceLevelId,
                                    SellableQuantities = offer.sellableQuantities,
                                    Charges = offer.charges != null ? (from charge in offer.charges
                                                                       select new Charge()
                                                                       {
                                                                           Reason = charge.reason,
                                                                           Type = charge.type,
                                                                           Amount = charge.amount
                                                                       }
                                               ).ToList() : null
                                }
                           ).ToList();
                }
            }

            return lstOffer;
        }

        public List<Offer> ParseOffers(string html)
        {
            var offersString = Regex.Match(html, @"storeUtils\['eventOfferJSON'\]=\[.*\]", RegexOptions.IgnoreCase).Value.Replace("storeUtils['eventOfferJSON']=", "");
            var offers = JsonConvert.DeserializeObject<List<Offer>>(offersString);
            return offers;
        }

        public List<Presale> ParsePresales(string html)
        {
            var presaleString = Regex.Match(html, @"storeUtils\['eventJSONData'\]=\{.*\}", RegexOptions.IgnoreCase).Value.Replace("storeUtils['eventJSONData']=", "");
            presaleString = Regex.Match(presaleString, @"presales\"":\[.*\],\""merchandises", RegexOptions.IgnoreCase).Value;
            presaleString = Regex.Match(presaleString, @"\[.*\]", RegexOptions.IgnoreCase).Value;

            var presales = JsonConvert.DeserializeObject<List<Presale>>(presaleString);
            return presales;
        }



        public List<Seat> ParseStandingSeats(string html)
        {
         /*
         HIDDEN
         */
        }

        public List<Seat> ParseSeats(string html)
        {
            var parser = new HtmlParser();
            var document = parser.Parse(html);
            var seatsHtml = document.QuerySelectorAll(".zoomer .map__svg .seats").FirstOrDefault();
            var seats = new List<Seat>();
            if (seatsHtml != null)
            {
                foreach (var section in seatsHtml.Children)
                {
                    foreach (var row in section.Children)
                    {
                        foreach (var seat in row.Children)
                        {
                            var seatData = new Seat()
                            {
                                Id = seat.GetAttribute("id"),
                                Name = seat.GetAttribute("data-seat-name"),
                                Row = row.GetAttribute("data-row-name"),
                                Section = section.GetAttribute("data-section-name")
                            };
                            seats.Add(seatData);
                        }
                    }
                }
            }
            return seats;
        }

        /// <summary>
        /// Parse the Seats Json into List of Seats
        /// </summary>
        /// <param name="htmlTask"></param>
        /// <returns></returns>
        public async Task<List<Seat>> ParseSeatsInfoJson(string htmlTask)
        {
            await Task.Delay(4);
            SegmentCollection objSegmentCollection = new SegmentCollection();
            var seats = new List<Seat>();

            //var html = htmlTask.Result;
            var html = htmlTask;

            if (!string.IsNullOrEmpty(html))
            {
                var seatString = Regex.Match(html, @"""pages"":\[.*\}]", RegexOptions.IgnoreCase).Value;
                objSegmentCollection = JsonConvert.DeserializeObject<SegmentCollection>("{" + seatString + "}");


                if (objSegmentCollection != null && objSegmentCollection.pages != null && objSegmentCollection.pages.Count > 0)
                {

                    foreach (var segments in objSegmentCollection.pages.First().segments)
                    {
                        //For Standing Seat
                        if (segments.generalAdmission.HasValue && ((bool)segments.generalAdmission))
                        {
                            var seatData = new Seat()
                            {
                                Id = segments.id,
                                Name = segments.name,
                                IsStanding = true
                            };
                            seats.Add(seatData);

                            continue;
                        }

                        //Check section existance
                        if (segments.segments != null)
                        {

                            //For Normal Seat
                            //Parallel.ForEach(segments.segments, section =>
                            foreach (var section in segments.segments)
                            {

                                //Check rows existance
                                if (section.segments != null)
                                {

                                    foreach (var rows in section.segments)
                                    {

                                        //Check seat existance
                                        if (rows.placesNoKeys != null)
                                        {
                                            int Counter = 0;
                                            foreach (var seat in rows.placesNoKeys)
                                            {
                                                Counter++;

                                                if (seat.Count > 1)
                                                {
                                                    var seatData = new Seat()
                                                    {

                                                        Id = Convert.ToString(seat[0]),
                                                        Name = Convert.ToString(seat[1]),
                                                        Row = rows.name,
                                                        Section = section.name
                                                    };
                                                    seats.Add(seatData);
                                                }

                                                if (Counter % 2 == 0)
                                                {
                                                    Thread.Sleep(1);
                                                }
                                            }
                                        }

                                    }
                                }
                                Thread.Sleep(10);
                            }
                        }

                    }
                }
            }

            return seats;
        }

        public List<Facet> ParseFacets(Task<string> htmlTask)
        {
            List<Facet> lstFacet = new List<Facet>();

            var html = htmlTask.Result;
            if (!string.IsNullOrEmpty(html))
            {
                var facetString = Regex.Match(html, @"""facets"":\[.*\}],""_links", RegexOptions.IgnoreCase).Value.Replace(@"""facets"":", "").Replace(@",""_links", "");
                lstFacet = JsonConvert.DeserializeObject<List<Facet>>(facetString);
            }

            return lstFacet;
        }

    }

    public enum SectionGrouping
    {
        Odd,
        Even,
        OneByOne
    }
}
