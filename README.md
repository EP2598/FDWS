# FlexiDev Witch Saga (FDWS)

A web application that calculates the average number of villager deaths caused by witches based on age and year of death data.

## Purpose

Helps villages analyze witch-related death patterns using Fibonacci algorithms to calculate supernatural fatality averages from historical data.

## Features

- Dynamic table interface for villager death records
- BigInteger Fibonacci calculations for large numbers
- Real-time input validation
- Responsive design
- Comprehensive unit & integration tests

## How It Works

1. Enter **Age of Death** and **Year of Death** for each villager
2. Application calculates birth year: `Year of Death - Age of Death`
3. Uses Fibonacci sequence on birth year to determine death patterns
4. Returns average deaths across all villagers

## Technology Stack

- ASP.NET Core 5.0 (C# 9.0)
- Bootstrap 5, JavaScript
- xUnit testing with FluentAssertions
