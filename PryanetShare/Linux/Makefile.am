SUBDIRS = Pixmaps

ASSEMBLY = PryanetShare
TARGET = exe

LINK = $(REF_PRYANETSHARE) $(NOTIFY_SHARP_LIBS) $(WEBKIT_SHARP_LIBS)

if HAVE_APP_INDICATOR
BUILD_DEFINES="-define:HAVE_APP_INDICATOR"
endif

SOURCES =  \
	../Program.cs \
	../PryanetAboutController.cs \
	../PryanetBubblesController.cs \
	../PryanetControllerBase.cs \
	../PryanetEventLogController.cs \
	../PryanetExtensions.cs \
	../PryanetInvite.cs \
	../PryanetKeys.cs \
	../PryanetPlugin.cs \
	../PryanetSetupController.cs \
	../PryanetStatusIconController.cs \
	PryanetAbout.cs \
	PryanetBubbles.cs \
	PryanetController.cs \
	PryanetEventLog.cs \
	PryanetSetup.cs \
	PryanetSetupWindow.cs \
	PryanetSpinner.cs \
	PryanetStatusIcon.cs \
	PryanetUI.cs \
	PryanetUIHelpers.cs

include $(top_srcdir)/build/build.mk

bin_SCRIPTS = pryanetshare

Applicationsdir = $(datadir)/applications
dist_Applications_DATA = pryanetshare.desktop \
	pryanetshare-invite-opener.desktop

install-data-hook:
	test -f $(datadir)/applications/defaults.list && \
	test -z $(DESTDIR) && update-desktop-database $(datadir)/applications || :
