#!/bin/env bash

echo "\nFetching Lucide info.json\n"
dotnet run --project source/lucide-build -- fetch-info latest

echo "\nBuilding Lucide\n"
dotnet run --project source/lucide-build -- generate-icon-usage source/lucide
