﻿using MassTransit;

namespace Shared.Events;

public class UserCreatedEvent : Event
{
    public string Firstname { get; set; }
    public string Surname { get; set; }
    public string UserId { get; set; }


   
}