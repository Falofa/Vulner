import os, sys
from os import system, makedirs

devenv = 'devenv'
vulner = 'Vulner.sln'
modes = ['Debug', 'Release']
arcs = ['Win32', 'Win64']

for mode in modes:
	for arc in arcs:
		print ( '%s/%s' % (mode, arc) )
		try:
			os.makedirs( 'Compiled\\%s\\%s' % (mode, arc) )
		except:
			pass
		cmd = '%s /Rebuild %s /ProjectConfig "%s|%s" "%s"' % ( devenv, mode, mode, arc, vulner )
		system(cmd)
		#exit()
		os.rename( 'Vulner\\bin\\%s\\Vulner.exe' % (mode), 'Compiled\\%s\\%s\\Vulner.exe' % (mode, arc) )