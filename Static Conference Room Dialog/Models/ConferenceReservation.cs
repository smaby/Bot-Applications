using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application2.Models
{   
    public enum ConferenceRoom
	{
		ConferenceA,
		ConferenceB,
		ConferenceC,
		ConferenceD
	}

	public enum AmmenitiesOptions
	{
        Example,
		Projector,
		WhiteBoard,
		ConferencePhone,
		Wifi
     
	}
    [Serializable]
    public class ConferenceReservation
    {
        public ConferenceRoom? Conference;
        public int? NumberOfEmployees;
        public DateTime? CheckInDate;
        public double? TotaltimeReq;
        public List<AmmenitiesOptions> Ammenities;

        public static IForm<ConferenceReservation> BuildForm()
        {
            return new FormBuilder<ConferenceReservation>()
                .Message("Welcome to the Conference Room reservation bot!")
                .Build();
        }

    }
}