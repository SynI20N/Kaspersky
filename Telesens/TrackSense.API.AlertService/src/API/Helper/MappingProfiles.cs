using AutoMapper;
using TrackSense.API.AlertService.Models;

namespace TrackSense.API.AlertService.Helper;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<NotificationGroup, NotificationGroupDto>();
        CreateMap<NotificationGroupDto, NotificationGroup>();
        CreateMap<TelegramChat, TelegramChatDto>();
        CreateMap<TelegramChatDto, TelegramChat>();
        CreateMap<Email, EmailDto>();
        CreateMap<EmailDto, Email>();
        CreateMap<AlertRule, AlertRuleDto>();
        CreateMap<AlertRuleDto, AlertRule>();
        CreateMap<AlertEvent, AlertEventDto>();
        CreateMap<AlertEventDto, AlertEvent>();
        CreateMap<AlertRuleNotificationGroup, AlertRuleNotificationGroupDto>();
        CreateMap<AlertRuleNotificationGroupDto, AlertRuleNotificationGroup>();
    }
}