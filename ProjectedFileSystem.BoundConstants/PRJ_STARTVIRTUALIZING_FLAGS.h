#pragma once
#include <projectedfslib.h>
namespace ProjectedFileSystem {
	namespace BoundConstants {
		public enum class PRJ_STARTVIRTUALIZING_FLAGS
		{
			PRJ_FLAG_NONE = ::PRJ_STARTVIRTUALIZING_FLAGS::PRJ_FLAG_NONE,
			PRJ_FLAG_USE_NEGATIVE_PATH_CACHE = ::PRJ_STARTVIRTUALIZING_FLAGS::PRJ_FLAG_USE_NEGATIVE_PATH_CACHE
		};
	}
}