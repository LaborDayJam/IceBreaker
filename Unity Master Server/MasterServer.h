#pragma once

// Network packet types
enum {
		// Master server messages start at 200, is reflected in the Unity message types
		ID_DATABASE_ROWID = 200,
		ID_MASTERSERVER_REDIRECT,
		ID_MASTERSERVER_MSG
};

static const int NATINDEX = 4;
static const int IPINDEX = 9;
static const int PORTINDEX = 10;
static const int GUIDINDEX = 12;

static const char* VERSION = "2.0.0b1";
static const int DEFAULTPORT = 23466;

enum 
{
	ERROR = 0,
	WARNING = 1,
	INFORMATIONAL = 2,
	DEBUG = 9
};

static int debugLevel = DEBUG;

void error_log(const char* format, ...);
void warn_log(const char* format, ...);
void stats_log(const char* format, ...);
void info_log(const char* format, ...);
void debug_log(const char* format, ...);

void print_log(const char* format, ...);
