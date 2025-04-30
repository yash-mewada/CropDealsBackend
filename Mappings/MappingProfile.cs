using AutoMapper;
using CropDeals.Models;
using CropDeals.Models.DTOs;

namespace CropDeals.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUpRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<ApplicationUser, SignUpRequest>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<SignInModel, ApplicationUser>();

            // mappings for Crops
            CreateMap<CropCreateDto, Crop>();
            CreateMap<CropUpdateDto, Crop>();

            // mappings for CropListings
            CreateMap<CropListingCreateDto, CropListing>();
            CreateMap<CropListingUpdateDto, CropListing>();

            // mappings for Subscription
            CreateMap<Subscription, SubscriptionReadDto>()
                .ForMember(dest => dest.CropName, opt => opt.MapFrom(src => src.Crop.Name));

            CreateMap<SubscriptionCreateDto, Subscription>();

            CreateMap<CropListing, CropListingReadDto>()
                .ForMember(dest => dest.CropName, opt => opt.MapFrom(src => src.Crop.Name))
                .ForMember(dest => dest.FarmerName, opt => opt.MapFrom(src => src.Farmer.Name));

        }
    }
}
