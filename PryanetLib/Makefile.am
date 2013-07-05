ASSEMBLY = PryanetLib
TARGET = library

ASSEMBLY_INFO_SOURCE = Defines.cs

SOURCES =  \
	Defines.cs \
	PryanetBackend.cs \
	PryanetConfig.cs \
	PryanetExtensions.cs \
	PryanetFetcherBase.cs \
	PryanetFetcherSSH.cs \
	PryanetListenerBase.cs \
	PryanetListenerFactory.cs \
	PryanetListenerTcp.cs \
	PryanetLogger.cs \
	PryanetRepoBase.cs \
	PryanetUser.cs \
	PryanetWatcher.cs \
	PryanetWrappers.cs


install-data-hook:
	for ASM in $(EXTRA_BUNDLE); do \
		$(INSTALL) -m 0755 $$ASM $(DESTDIR)$(moduledir); \
	done;

uninstall-hook:
	for ASM in $(EXTRA_BUNDLE); do \
		rm -f $(DESTDIR)$(moduledir)/`basename $$ASM`; \
	done;

include $(top_srcdir)/build/build.mk
