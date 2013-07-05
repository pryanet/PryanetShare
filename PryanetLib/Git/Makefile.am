ASSEMBLY = PryanetLib.Git
TARGET = library

ASSEMBLY_INFO_SOURCE = ../Defines.cs

LINK = -r:$(DIR_BIN)/PryanetLib.dll

SOURCES =  \
	PryanetGit.cs \
	PryanetFetcherGit.cs \
	PryanetRepoGit.cs


install-data-hook:
	for ASM in $(EXTRA_BUNDLE); do \
		$(INSTALL) -m 0755 $$ASM $(DESTDIR)$(moduledir); \
	done;

uninstall-hook:
	for ASM in $(EXTRA_BUNDLE); do \
		rm -f $(DESTDIR)$(moduledir)/`basename $$ASM`; \
	done;

include $(top_srcdir)/build/build.mk
