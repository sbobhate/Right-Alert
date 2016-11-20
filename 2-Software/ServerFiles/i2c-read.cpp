
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

#include "Adafruit_INA219.h"

int main(int argc, char** argv)
{
	Adafruit_INA219 ina219;
	ina219.begin();


		float shuntvoltage = 0;
		float busvoltage = 0;
		float current_mA = 0;
		float loadvoltage = 0;
		//do
		{
		shuntvoltage = ina219.getShuntVoltage_mV();
		busvoltage = ina219.getBusVoltage_V();
		current_mA = ina219.getCurrent_mA();
		loadvoltage = busvoltage + (shuntvoltage / 1000);

		//Serial.print("Bus Voltage:   "); Serial.print(busvoltage); Serial.println(" V");
		//fprintf(stdout, "Bus Voltage:   %.3f V\n", busvoltage);
		//fprintf(stdout, "Shunt Voltage: %.3f mV\n", shuntvoltage);
		//fprintf(stdout, "Load Voltage:  %.3f V\n", loadvoltage);
		//fprintf(stdout, "Current:       %.3f mA\n", current_mA);
		//fprintf(stdout, "Watts:         %.3f W\n", (current_mA/1000)*busvoltage);
		//fprintf(stdout, "\n");

		FILE * fp;
		fp = fopen ("/home/ubuntu/Desktop/bandhan/SQL-Scripts/PowerParams.txt", "w+");
		fprintf(fp, "%.3f %.3f %.3f %.3f %.3f", busvoltage, shuntvoltage, loadvoltage, current_mA, (current_mA/1000*busvoltage));
		fclose(fp);
		usleep(1000000);

		system("(sudo /home/ubuntu/Desktop/bandhan/SQL-Scripts/PowerUpdate-SQL $(cat /home/ubuntu/Desktop/bandhan/SQL-Scripts/PowerParams.txt) &) > /dev/null");

		// if battery voltage < threshold then system("sudo shutdown -h 0")
		if (busvoltage < 11.10) system("sudo shutdown -h 0");
		} 
		//while (true);

	return 0;
}
