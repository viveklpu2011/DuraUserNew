﻿using System;
using System.Collections.Generic;

namespace DuraApp.Core.Models.ResponseModels
{
    public class GoogleTimeResponseModel
    {
        public List<string> destination_addresses { get; set; }
        public List<string> origin_addresses { get; set; }
        public List<Row> rows { get; set; }
        public string status { get; set; }
    }
    public class Distance
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class Duration
    {
        public string text { get; set; }
        public int value { get; set; }
    }
    public class Element
    {
        public Distance distance { get; set; }
        public Duration duration { get; set; }
        public string status { get; set; }
    }
    public class Row
    {
        public List<Element> elements { get; set; }
    }
}
