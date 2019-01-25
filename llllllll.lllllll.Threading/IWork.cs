using System.Threading.Tasks;

namespace *********.**********.Threading
{
   public interface IWork
	{
      /// <summary>
      ///   Perform the work.
      /// </summary>
      /// <remarks>
      ///   <b>Perform</b> does the processing of work. 
      /// </remarks>
      Task Perform();
   }
}
