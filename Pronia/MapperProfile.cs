using AutoMapper;

namespace Pronia
{
	public class MapperProfile:Profile
	{
		public MapperProfile()
		{
			CreateMap<Category, CreateCategoryVM>();
			CreateMap<CreateCategoryVM, Category>();
        }
	}
}

