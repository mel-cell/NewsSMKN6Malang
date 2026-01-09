.PHONY: build run clean

build:
	dotnet build
	dotnet run

dev:
	dotnet watch run

clean:
	dotnet clean
	rm -rf bin obj
