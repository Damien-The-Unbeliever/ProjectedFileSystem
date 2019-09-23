#pragma once
#include "PRJ_NOTIFY_TYPES.h"
#include "PRJ_NOTIFICATION.h"
#include "PRJ_STARTVIRTUALIZING_FLAGS.h"
#include "PRJ_UPDATE_TYPES.h"
#include "PRJ_UPDATE_FAILURE_CAUSES.h"
#include "PRJ_FILE_STATE.h"
#include "PRJ_CALLBACK_DATA_FLAGS.h"
#include "PRJ_COMPLETE_COMMAND_TYPE.h"

using namespace System;
namespace ProjectedFileSystem
{
	namespace BoundConstants {
		public ref class Boundary
		{
		public:
			static const int PRJ_PLACEHOLDER_ID_LENGTH = ::PRJ_PLACEHOLDER_ID::PRJ_PLACEHOLDER_ID_LENGTH;
			constexpr static const size_t PRJ_COMPLETE_COMMAND_EXTENDED_PARAMETERS_DirEntry = offsetof(PRJ_COMPLETE_COMMAND_EXTENDED_PARAMETERS, Enumeration.DirEntryBufferHandle);
			constexpr static const size_t PRJ_COMPLETE_COMMAND_EXTENDED_PARAMETERS_NotificationMask = offsetof(PRJ_COMPLETE_COMMAND_EXTENDED_PARAMETERS, Notification.NotificationMask);
			static const HRESULT ERROR_REPARSE_TAG_MISMATCH_VALUE = HRESULT_FROM_WIN32(ERROR_REPARSE_TAG_MISMATCH);
			static const HRESULT ERROR_FILE_NOT_FOUND_VALUE = HRESULT_FROM_WIN32(ERROR_FILE_NOT_FOUND);
			static const HRESULT ERROR_IO_PENDING_VALUE = HRESULT_FROM_WIN32(ERROR_IO_PENDING);
			static const HRESULT ERROR_INSUFFICIENT_BUFFER_VALUE = HRESULT_FROM_WIN32(ERROR_INSUFFICIENT_BUFFER);
			static const HRESULT S_OK_VALUE = S_OK;

			property PRJ_NOTIFY_TYPES PRJ_NOTIFY_TYPES {
				ProjectedFileSystem::BoundConstants::PRJ_NOTIFY_TYPES get() {
					return ProjectedFileSystem::BoundConstants::PRJ_NOTIFY_TYPES::PRJ_NOTIFY_NONE;
				}
			}
			property PRJ_NOTIFICATION PRJ_NOTIFICATION {
				ProjectedFileSystem::BoundConstants::PRJ_NOTIFICATION get() {
					return ProjectedFileSystem::BoundConstants::PRJ_NOTIFICATION::PRJ_NOTIFICATION_FILE_OPENED;
				}
			}
			property PRJ_STARTVIRTUALIZING_FLAGS PRJ_STARTVIRTUALIZING_FLAGS {
				ProjectedFileSystem::BoundConstants::PRJ_STARTVIRTUALIZING_FLAGS get() {
					return ProjectedFileSystem::BoundConstants::PRJ_STARTVIRTUALIZING_FLAGS::PRJ_FLAG_NONE;
				}
			}
			property PRJ_UPDATE_TYPES PRJ_UPDATE_TYPES {
				ProjectedFileSystem::BoundConstants::PRJ_UPDATE_TYPES get() {
					return ProjectedFileSystem::BoundConstants::PRJ_UPDATE_TYPES::PRJ_UPDATE_NONE;
				}
			}
			property PRJ_UPDATE_FAILURE_CAUSES PRJ_UPDATE_FAILURE_CAUSES {
				ProjectedFileSystem::BoundConstants::PRJ_UPDATE_FAILURE_CAUSES get() {
					return ProjectedFileSystem::BoundConstants::PRJ_UPDATE_FAILURE_CAUSES::PRJ_UPDATE_FAILURE_CAUSE_NONE;
				}
			}
			property PRJ_FILE_STATE PRJ_FILE_STATE {
				ProjectedFileSystem::BoundConstants::PRJ_FILE_STATE get() {
					return ProjectedFileSystem::BoundConstants::PRJ_FILE_STATE::PRJ_FILE_STATE_TOMBSTONE;
				}
			}
			property PRJ_CALLBACK_DATA_FLAGS PRJ_CALLBACK_DATA_FLAGS {
				ProjectedFileSystem::BoundConstants::PRJ_CALLBACK_DATA_FLAGS get() {
					return ProjectedFileSystem::BoundConstants::PRJ_CALLBACK_DATA_FLAGS::PRJ_CB_DATA_FLAG_ENUM_RESTART_SCAN;
				}
			}
			property PRJ_COMPLETE_COMMAND_TYPE PRJ_COMPLETE_COMMAND_TYPE {
				ProjectedFileSystem::BoundConstants::PRJ_COMPLETE_COMMAND_TYPE get() {
					return ProjectedFileSystem::BoundConstants::PRJ_COMPLETE_COMMAND_TYPE::PRJ_COMPLETE_COMMAND_TYPE_ENUMERATION;
				}
			}
		};
	}
}
