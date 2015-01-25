#include "TableSerializer.h"
#include "BitStream.h"
#include "StringCompressor.h"
#include "DS_Table.h"
#include "LightweightDatabaseServer.h"
#include "RakPeerInterface.h"
#include "RakNetworkFactory.h"
#include "RakSleep.h"
#include "MessageIdentifiers.h"
#include "MasterServer.h"
#include <signal.h>
#include <string>
#include <stdarg.h>

#ifdef WIN32
#include <stdio.h>
#include <time.h>
#include <windows.h>
#else
#include <stdlib.h>
#endif

const int MAX_LOG_NAME_SIZE = 50;

LightweightDatabaseServer databaseServer;
FILE* outputStream = stdout;
char* logfile = "masterserver.log";
bool quit;
bool printStats = false;

void error_log(const char* format, ...)
{
	if (debugLevel >= ERROR)
	{
		if (!outputStream)
			return;
		time_t rawtime;
		struct tm * timeinfo;
		time ( &rawtime );
		timeinfo = localtime ( &rawtime );
		fprintf(outputStream, "%02d-%02d-%d %02d:%02d:%02d\t-ERROR-\t",timeinfo->tm_mday, 1+timeinfo->tm_mon, 1900+timeinfo->tm_year, timeinfo->tm_hour, timeinfo->tm_min, timeinfo->tm_sec);
		
		va_list va;
		va_start( va, format );
		vfprintf(outputStream, format, va);
	}
}

void warn_log(const char* format, ...)
{
	if (debugLevel >= WARNING)
	{
		if (!outputStream)
			return;
		time_t rawtime;
		struct tm * timeinfo;
		time ( &rawtime );
		timeinfo = localtime ( &rawtime );
		fprintf(outputStream, "%02d-%02d-%d %02d:%02d:%02d\t-WARN-\t",timeinfo->tm_mday, 1+timeinfo->tm_mon, 1900+timeinfo->tm_year, timeinfo->tm_hour, timeinfo->tm_min, timeinfo->tm_sec);
		
		va_list va;
		va_start( va, format );
		vfprintf(outputStream, format, va);
	}
}

void stats_log(const char* format, ...)
{
	if (debugLevel >= ERROR && printStats)
	{
		if (!outputStream)
			return;
		time_t rawtime;
		struct tm * timeinfo;
		time ( &rawtime );
		timeinfo = localtime ( &rawtime );
		fprintf(outputStream, "%02d-%02d-%d %02d:%02d:%02d\t-STATS-\t",timeinfo->tm_mday, 1+timeinfo->tm_mon, 1900+timeinfo->tm_year, timeinfo->tm_hour, timeinfo->tm_min, timeinfo->tm_sec);
		
		va_list va;
		va_start( va, format );
		vfprintf(outputStream, format, va);
	}
}

void info_log(const char* format, ...)
{
	if (debugLevel >= INFORMATIONAL)
	{
		if (!outputStream)
			return;
		time_t rawtime;
		struct tm * timeinfo;
		time ( &rawtime );
		timeinfo = localtime ( &rawtime );
		fprintf(outputStream, "%02d-%02d-%d %02d:%02d:%02d\t-INFO-\t",timeinfo->tm_mday, 1+timeinfo->tm_mon, 1900+timeinfo->tm_year, timeinfo->tm_hour, timeinfo->tm_min, timeinfo->tm_sec);
		
		va_list va;
		va_start( va, format );
		vfprintf(outputStream, format, va);
	}
}

void debug_log(const char* format, ...)
{
	if (debugLevel >= DEBUG)
	{
		if (!outputStream)
			return;
		time_t rawtime;
		struct tm * timeinfo;
		time ( &rawtime );
		timeinfo = localtime ( &rawtime );
		fprintf(outputStream, "%02d-%02d-%d %02d:%02d:%02d\t-DEBUG-\t",timeinfo->tm_mday, 1+timeinfo->tm_mon, 1900+timeinfo->tm_year, timeinfo->tm_hour, timeinfo->tm_min, timeinfo->tm_sec);
		
		va_list va;
		va_start( va, format );
		vfprintf(outputStream, format, va);
	}
}
			  
void print_log(const char* format, ...)
{
	if (!outputStream)
		return;
	time_t rawtime;
	struct tm * timeinfo;
	time ( &rawtime );
	timeinfo = localtime ( &rawtime );
	fprintf(outputStream, "%02d-%02d-%d %02d:%02d:%02d\t",timeinfo->tm_mday, 1+timeinfo->tm_mon, 1900+timeinfo->tm_year, timeinfo->tm_hour, timeinfo->tm_min, timeinfo->tm_sec);

	va_list va;
	va_start( va, format );
	vfprintf(outputStream, format, va);
}

void shutdown(int sig)
{
	print_log("Shutting down\n\n");
	quit = true;
}

void rotateLogFile(int sig)
{
	if (logfile != NULL)
	{	
		char savedLogFile[MAX_LOG_NAME_SIZE];
		fclose(outputStream);		// Does a flush internally
		time_t currentTime = time(0);
		if (strftime( savedLogFile, MAX_LOG_NAME_SIZE, "masterserver_%d%m%y%H%M%S.log", localtime(&currentTime) ) == 0)
			print_log("Error creating new log file");
		rename(logfile, savedLogFile);
		outputStream = fopen(logfile, "a");
#ifndef WIN32
		setlinebuf(outputStream);
#endif
	}
	else
	{
		print_log("Log file name not set, cannot rotate");
	}
}

void usage()
{
	printf("\nAccepted parameters are:\n\t-m\tMaster server port (1-65535)\n\t-f\t-s\tStatistics print delay (minutes)\n\t-c\tMax connection count\n\t-l\tSend output to log file\n\nIf any parameter is omitted the default value is used.\n");
}

void RemoveTable(std::string table)
{
	if (table == "") return;	
	if (databaseServer.RemoveTable(const_cast<char*>(table.c_str())))
		print_log("Success\n");
	else
		print_log("Table %s not found\n", table.c_str());
}

void ProcessPacket(Packet *packet)
{
	switch (packet->data[0])
	{
		case ID_DISCONNECTION_NOTIFICATION:
			info_log("%s has diconnected\n", packet->systemAddress.ToString());
			break;
		case ID_CONNECTION_LOST:
			info_log("Connection to %s lost\n", packet->systemAddress.ToString());
			break;
		case ID_NO_FREE_INCOMING_CONNECTIONS:
			error_log("No free incoming connection for %s\n", packet->systemAddress.ToString());
			break;
		case ID_NEW_INCOMING_CONNECTION:
			info_log("New connection established to %s\n", packet->systemAddress.ToString());
			break;
		case ID_CONNECTION_REQUEST_ACCEPTED:
			info_log("Connection to %s accepted\n", packet->systemAddress.ToString());
			break;
		case ID_CONNECTION_ATTEMPT_FAILED:
			error_log("Connection attempt to %s failed\n", packet->systemAddress.ToString());
			break;
		case ID_DATABASE_UNKNOWN_TABLE:
			error_log("ID_DATABASE_UNKNOWN_TABLE to %s\n", packet->systemAddress.ToString());
			break;
		case ID_DATABASE_INCORRECT_PASSWORD:
			error_log("ID_DATABASE_INCORRECT_PASSWORD to %s\n", packet->systemAddress.ToString());
			break;
		case ID_DATABASE_QUERY_REPLY:
			info_log("ID_DATABASE_QUERY_REPLY to %s\n", packet->systemAddress.ToString());
			break;
		default:
			error_log("Unknown ID %d from %s\n", packet->data[0], packet->systemAddress.ToString());
	}
}

int main(int argc, char *argv[])
{  
#ifndef WIN32
	setlinebuf(stdout);
#endif
	outputStream = stdout;

	RakPeerInterface *masterPeer = RakNetworkFactory::GetRakPeerInterface();
	
	// Default values
	int masterserverPort = DEFAULTPORT;
	int connectionCount = 1000;

	time_t timerInterval = 10;	// 60 seconds
	time_t rotateCheckTimer = time(0) + timerInterval;
	int rotateSizeLimit = 50000000;	// 50 MB
	bool useLogFile = false;
			
	// Process command line arguments
	for (int i = 1; i < argc; i++)
	{
		if (strlen(argv[i]) == 2 && argc>=i+1)
		{
			switch (argv[i][1]) 
			{
				case 'm':
				{
					masterserverPort = atoi(argv[i+1]);
					i++; // eat port number parameter
					if (masterserverPort < 1 || masterserverPort > 65535)
					{
						fprintf(stderr, "Master server port is invalid, should be between 0 and 65535.\nIt is also advisable to use a number above well known ports (>1024).\n");
						return 1;
					}
					break;
				}
				case 's':
				{
					int statDelay =  atoi(argv[i+1]);
					i++;
					if (statDelay < 0)
					{
						fprintf(stderr, "Statistics print delay must not be lower than 0. Use 0 to disable.\n");
						return 1;
					}
					databaseServer.SetStatDelay(statDelay);
					printStats = true;
					break;
				}
				case 'c':
				{
					connectionCount = atoi(argv[i+1]);
					i++;
					if (connectionCount < 0)
					{
						fprintf(stderr, "Connection count must be higher than 0.\n");
						return 1;
					}
					break;
				}
				case 'l':
				{				
					useLogFile = true;
					outputStream = fopen(logfile, "a");
#ifndef WIN32
					setlinebuf(outputStream);
#endif
					break;
				}
				case 'e':
				{
					debugLevel = atoi(argv[i+1]);
					i++;
					if (debugLevel < 0 || debugLevel > 9)
					{
						fprintf(stderr, "Log level can be 0(errors), 1(warnings), 2(informational), 9(debug)\n");
						return 1;
					}
					break;
				}
				case '?':
					usage();
					return 0;
				default:
					fprintf(stderr, "Parsing error, unknown parameter\n\n");
					usage();
					return 1;
			}
		}
		else
		{
			printf("Parsing error, incorrect parameters\n\n");
			usage();
			return 1;
		}
	}

	// Start the peers
	masterPeer->SetMaximumIncomingConnections(connectionCount);
	SocketDescriptor sd(masterserverPort,0);
	masterPeer->Startup(connectionCount, 30, &sd, 1);
	masterPeer->AttachPlugin(&databaseServer);
		
	print_log("Unity master server version %s\n", VERSION);
	print_log("Master server port set to %d\n",masterserverPort);
	print_log("%d connection count limit\n", connectionCount);
	if (printStats)
		print_log("%d sec delay between statistics print to log\n", databaseServer.GetStatDelay());
	
	// Register signal handlers
#ifndef WIN32
	if (signal(SIGHUP, rotateLogFile) == SIG_ERR)
		error_log("Problem setting up hangup signal handler");
#endif
	if (signal(SIGINT, shutdown) == SIG_ERR || signal(SIGTERM, shutdown) == SIG_ERR)
		error_log("Problem setting up terminate signal handler");
	else
		print_log("To quit press Ctrl-C\n----------------------------------------------------\n");

	Packet *p;
	while (!quit)
	{
		p=masterPeer->Receive();
		while (p)
		{
			ProcessPacket(p);
			masterPeer->DeallocatePacket(p);
			p=masterPeer->Receive();
		}
		// Is it time to rotate the logs?
		if (useLogFile)
		{
			if (rotateCheckTimer < time(0))
			{
				// We should always be writing to the end of the stream, so the current position should give the total size
				long position = ftell(outputStream);
				if (position > rotateSizeLimit) {
					info_log("Rotating logs, size limit reached\n");
					rotateLogFile(0);
				}
				rotateCheckTimer = time(0) + timerInterval;
			}
		}
		RakSleep(30);
	}

	// Clean up
	masterPeer->Shutdown(100,0);
	RakNetworkFactory::DestroyRakPeerInterface(masterPeer);
	fclose(outputStream);
				
	return 0;
}
