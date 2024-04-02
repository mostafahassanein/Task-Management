using AutoMapper;
using TaskManagement.WebAPI.Models.DTOs;
using TaskManagement.WebAPI.Models.Requests;
using TaskManagement.WebAPI.Models.Responses;

namespace TaskManagement.WebAPI.Mapper
{
    public class MappingProfiles : Profile 
    {
        public MappingProfiles()
        {
            CreateMap<AddTaskRequest, TaskManagement.Entities.Tasks>()
                .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.taskId))
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.taskName))
                .ForPath(dest => dest.Description, opt => opt.MapFrom(src => src.taskDesc))
                .ForPath(dest => dest.DueDate, opt => opt.MapFrom(src => src.dueDate))
                .ForPath(dest => dest.Username, opt => opt.MapFrom(src => src.username))
                .ForPath(dest => dest.Status, opt => opt.MapFrom(src => src.status))
                .ForPath(dest => dest.Priority, opt => opt.MapFrom(src => src.priority));

            CreateMap<SendMessageRequest, TaskManagement.Entities.Notification>()
                .ForPath(dest => dest.From, opt => opt.MapFrom(src => src.from))
                .ForPath(dest => dest.To, opt => opt.MapFrom(src => src.to))
                .ForPath(dest => dest.Message, opt => opt.MapFrom(src => src.message));

            CreateMap<ChatMessageDTO, TaskManagement.Entities.ChatMessage>();

            CreateMap<TaskManagement.Entities.Tasks, GetTaskResponse>();
            CreateMap<TaskManagement.Entities.ChatMessage, GetChatHistoryResponse>();
        }
    }
}
