# PersianMonthView

**A Persian month view date picker (independent of system calendar and locale).**

## Features
- Supports **Persian calendar** (Jalali).
- Allows **quick date selection** with an interactive UI.
- Includes **year, month, and day selection popups**.

## Installation
Install via NuGet:
```sh
dotnet add package PersianMonthView
```
Or, using Visual Studio:
1. Open **Manage NuGet Packages**.
2. Search for `PersianMonthView`.
3. Click **Install**.

## Usage Example
```csharp
var persianDatePicker = new PersianMonthView();
Controls.Add(persianDatePicker);

// Get selected date
DateTime selectedDate = persianDatePicker.SelectedDate;
PersianDateTime persianSelectedDate = persianDatePicker.SelectedPersianDate;
```

## License
This package is licensed under the **MIT License**.
