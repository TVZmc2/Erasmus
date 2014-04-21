using System.Collections.Generic;

namespace ErasmusApp.ViewModel.Event
{
    class EventModel
    {
        public List<EventData> Events { get; set; }
        public bool isDataLoaded { get; set; }

        public void LoadData()
        {
            Events = CreateEventData();
            isDataLoaded = true;
        }

        private List<EventData> CreateEventData()
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
