#!bin/bash

echo
PROMPT_COMMAND='echo -en "\033]0;LINUX SIMULATOR TEST\a"'
eval $PROMPT_COMMAND

for f in ../bin/Debug/*.stl; do	
	com="sudo mono ../bin/Debug/RocketSimulator.exe -stl ${f}"
	#echo $com
	eval $com
	echo
done

STR=$"Press any key to continue..."
read -n 1 -s -r -p "${STR}"
echo
