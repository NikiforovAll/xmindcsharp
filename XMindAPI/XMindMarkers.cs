
using System;
using System.ComponentModel;

namespace XMindAPI
{
    /// <summary>
    /// XMindMarkers define optional markers that can be added to topics. Markers are displayed right in front of the topic title.
    /// Refer AddMarker() method.
    /// </summary>
    public enum XMindMarkers
    {
        // Task markers:
        [Description("task-start")]
        TaskStart,
        [Description("task-quarter")]
        TaskQuarter,
        [Description("task-half")]
        TaskHalf,
        [Description("task-3quar")]
        Task3Quarter,
        [Description("task-done")]
        TaskDone,
        [Description("task-paused")]
        TaskPaused,
        // Priority markers:
        [Description("priority-1")]
        Priority1,
        [Description("priority-2")]
        Priority2,
        [Description("priority-3")]
        Priority3,
        [Description("priority-4")]
        Priority4,
        [Description("priority-5")]
        Priority5,
        [Description("priority-6")]
        Priority6,
        // Smiley markers:
        [Description("smiley-smile")]
        SmieySmile,
        [Description("smiley-laugh")]
        SmileyLaugh,
        [Description("smiley-angry")]
        SmileyAngry,
        [Description("smiley-cry")]
        SmileyCry,
        [Description("smiley-surprise")]
        SmileySurprise,
        [Description("smiley-boring")]
        SmileyBoring,
        // Flag markers:
        [Description("flag-green")]
        FlagGreen,
        [Description("flag-red")]
        FlagRed,
        [Description("flag-orange")]
        FlagOrange,
        [Description("flag-purple")]
        FlagPurple,
        [Description("flag-blue")]
        FlagBlue,
        [Description("flag-black")]
        FlagBlack,
        // Star markers:
        [Description("star-green")]
        StarGreen,
        [Description("star-red")]
        StarRed,
        [Description("star-yellow")]
        StarYellow,
        [Description("star-purple")]
        StarPurple,
        [Description("star-blue")]
        StarBlue,
        [Description("star-gray")]
        StarGray,
        // Half Star markers:
        [Description("half-star-green")]
        HalfStarGreen,
        [Description("half-star-red")]
        HalfStarRed,
        [Description("half-star-yellow")]
        HalfStarYellow,
        [Description("half-star-purple")]
        HalfStarPurple,
        [Description("half-star-blue")]
        HalfStarBlue,
        [Description("half-star-gray")]
        HalfStarGray,
        // Other markers:
        [Description("other-calendar")]
        Caledar,
        [Description("other-email")]
        Email,
        [Description("other-phone")]
        Phone,
        [Description("other-phone")]
        Phone2,
        [Description("other-fax")]
        Fax,
        [Description("other-people")]
        People,
        [Description("other-people2")]
        People2,
        [Description("other-clock")]
        Clock,
        [Description("other-coffee-cup")]
        CoffeeCup,
        [Description("other-question")]
        Question,
        [Description("other-exclam")]
        ExclamationMark,
        [Description("other-lightbulb")]
        LightBulb,
        [Description("other-businesscard")]
        BusinessCard,
        [Description("other-social")]
        Social,
        [Description("other-chat")]
        Chat,
        [Description("other-note")]
        Note,
        [Description("other-lock")]
        Lock,
        [Description("other-unlock")]
        Unlock,
        [Description("other-yes")]
        Yes,
        [Description("other-no")]
        No,
        [Description("other-bomb")]
        Bomb
    }
}