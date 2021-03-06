﻿using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blazor.GoogleMap.Map.Events
{
    public class MouseEventsInvoker : IMouseEventsInovkable
    {
        private readonly Dictionary<string, EventCallback<MouseEventArgs>> eventCallbacks = new Dictionary<string, EventCallback<MouseEventArgs>>();

        [JSInvokable]
        public Task InvokeMouseEvent(string eventName, MouseEventArgs mouseEventArgs)
        {
            if (eventCallbacks.TryGetValue(eventName, out var eventCallback))
            {
                return eventCallback.InvokeAsync(mouseEventArgs);
            }

            return Task.CompletedTask;
        }

        public IMouseEventsInovkable RegisterCallback(MouseEvent mouseEvent, EventCallback<MouseEventArgs> eventCallback)
        {
            if (!eventCallback.HasDelegate)
            {
                return this;
            }

            var mouseEventName = mouseEvent.ToString().ToLower();
            if (eventCallbacks.ContainsKey(mouseEventName))
            {
                eventCallbacks[mouseEventName] = eventCallback;
            }
            else
            {
                eventCallbacks.Add(mouseEventName, eventCallback);
            }

            return this;
        }

        [JSInvokable]
        public List<string> GetMouseEvents()
            => eventCallbacks.Keys.ToList();
    }
}
