using System;
using System.Collections.Generic;
using System.Text;

namespace DuraApp.Core.Helpers.Enums
{
    public enum MiscellaneousEnumeration
    {
    }
    public enum AppState
    {
        Undefinded,
        Foreground,
        Background
    }
    public enum ResultType
    {
        Ok,
        Invalid,
        Unauthorized,
        InternalError,
        PartialOk,
        NotFound,
        Unexpected
    }
    public enum ProfilePicSelectionType
    {
        Camera,
        Gallery
    }
    public enum TransitionType
    {
        SlideFromBottom = 0,
        None = 1,
        Default = 2,
    }
    public enum SendInvite
    {
        LOGIN_WAY = 0,
        HOME_WAY = 1,
        NONE = 2,
        MENU = 3,
        OTP = 4,
        SELECTEDLOCATION = 5,
        MAPLOCATION = 6,
        VERIFICATION = 7,
        PROMO = 8
    }
    public enum PaymentWay
    {
        BILLING_WAY = 0,
        HOME_MEMBERSHIP_WAY = 1,
        NONE = 2
    }

    public enum DocumentType
    {
        ID,
        Driving,
        Business1,
        Business2
    }
    public enum DeviceType
    {
        Android = 1,
        iOS = 2
    }
    public enum ExpressType
    {
        PickupLocation,
        WhereToLocation,
        StopLocation,
        AddAddress
    }
    public enum DocumentIdType
    {
        ID = 1,
        Driving = 2,
        Business1 = 3,
        Business2 = 4
    }
    public enum OrderStatusType
    {
        Pending = 1,
        Ongoing = 2,
        Completed = 3,
        Cancelled = 4,

    }
}
