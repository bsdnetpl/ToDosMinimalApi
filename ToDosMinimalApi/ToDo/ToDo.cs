﻿namespace ToDosMinimalApi.ToDo
{
    public class ToDo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Value { get; set; }

        public bool isComplited { get; set; }
    }
}