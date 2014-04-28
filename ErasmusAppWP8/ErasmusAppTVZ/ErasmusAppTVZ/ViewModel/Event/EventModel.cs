using System.Collections.Generic;

namespace ErasmusAppTVZ.ViewModel.Event
{
    class EventModel
    {
        public List<EventData> Events { get; set; }

        public static List<EventData> CreateEventData()
        {
            List<EventData> eventDataList = new List<EventData>();

            for (int i = 0; i < 10; i++)
            {
                EventData eventData = new EventData() 
                {
                    Title = "Event title " + i,
                    Description = "Sample description of selected event\n" +
                        "Sample description of selected event\n" +
                        "Sample description of selected event\n" +
                        "Sample description of selected event"
                };

                eventDataList.Add(eventData);
            }

            return eventDataList;
        }
    }
}
