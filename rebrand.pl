#!/usr/bin/perl

use strict;
use warnings;

=head Description

Recursively change files and references from PryanetShare to PryanetShare

=cut

if ( ! $ENV{'I_KNOW_WHAT_I_AM_DOING'} )
{
	print "You don't know what you are doing!\n";
	exit;
}

my $initial_path = `pwd`;
chomp($initial_path);
&correct_files($initial_path);

sub correct_files
{
	my $path = shift;
	my @files = `ls $path`;

	foreach ( @files )
	{
		chomp;
		next if m/^rebrand.pl$/;
		if ( -f "$path/$_" )
		{
			open FILE, "<$path/$_";
			my @lines = <FILE>;
			foreach my $line ( @lines )
			{
				chomp $line;
				$line =~ s/Sparkle/Pryanet/g;
				$line =~ s/SPARKLE/PRYANET/g;
				$line =~ s/sparkle/pryanet/g;
			}
			close FILE;

			open NEWFILE, ">$path/$_";
			foreach my $line ( @lines )
			{
				print NEWFILE $line;
				print NEWFILE "\n";
			}
			close NEWFILE;
		}
		elsif ( -d "$path/$_" )
		{
			correct_files("$path/$_");
		}

		if ( m/sparkle/i )
		{
			#fixme this is like cutting off the branch you are sitting on 
			my $new_file = $_;
			$new_file =~ s/Sparkle/Pryanet/g;
			$new_file =~ s/SPARKLE/PRYANET/g;
			$new_file =~ s/sparkle/pryanet/g;
			system('git', 'mv', "$path/$_", "$path/$new_file") or die "git mv $path/$_ $path/$new_file \n $!";
		}
	}
}
