.PHONY: build run clean

build:
	dotnet build
	dotnet run

clean:
	dotnet clean
	rm -rf bin obj
