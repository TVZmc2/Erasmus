using System.Collections.Generic;

namespace ErasmusAppTVZ.ViewModel.Event
{
    class EventModel
    {
        public static List<EventData> CreateEventData()
        {
            List<EventData> eventDataList = new List<EventData>();

            for (int i = 0; i < 20; i++)
            {
                EventData eventData = new EventData() 
                {
                    Title = "Event title " + i
                };

                eventDataList.Add(eventData);
            }

            return eventDataList;
        }
    }
}
