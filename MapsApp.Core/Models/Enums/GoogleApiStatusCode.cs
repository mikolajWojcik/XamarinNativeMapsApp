using MapsApp.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MapsApp.Models.Enums
{
    public enum GoogleApiStatusCode
    {
        [EnumHelperAttribute(DisplayName = "OK status")]
        OK,

        [EnumHelperAttribute(DisplayName = "Unknown error, please type searched text again")]
        UNKNOWN_ERROR,

        [EnumHelperAttribute(DisplayName = "Now address found, please search different address")]
        ZERO_RESULTS,

        [EnumHelperAttribute(DisplayName = "Limit reached, please contact developers team")]
        OVER_QUERY_LIMIT,

        [EnumHelperAttribute(DisplayName = "Access denied, please contact developers team")]
        REQUEST_DENIED,

        [EnumHelperAttribute(DisplayName = "Invalid request, please contact developers team")]
        INVALID_REQUEST,

        [EnumHelperAttribute(DisplayName = "Not found, please contact developers team")]
        NOT_FOUND
    }
}
