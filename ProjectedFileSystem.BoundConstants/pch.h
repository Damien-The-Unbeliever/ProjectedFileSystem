// pch.h: This is a precompiled header file.
// Files listed below are compiled only once, improving build performance for future builds.
// This also affects IntelliSense performance, including code completion and many code browsing features.
// However, files listed here are ALL re-compiled if any one of them is updated between builds.
// Do not add files here that you will be updating frequently as this negates the performance advantage.

#ifndef PCH_H
#define PCH_H

// prevent redefinition of NTSTATUS messages
#define UMDF_USING_NTSTATUS

#include <windows.h>
#include <objbase.h>    // For CoCreateGuid

#include <ntstatus.h>   // For STATUS_CANNOT_DELETE

// STL
#include <string>
#include <map>
#include <vector>
#include <algorithm>
#include <memory>

// Windows SDK
#include <projectedfslib.h>

#endif //PCH_H
