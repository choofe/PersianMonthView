# انتخابگر تاریخ فارسی (ماهانه)
**تقویم فارسی ماهانه با قابلیت انتخاب تاریخ و استفاده از تاریخ انتخاب شده که مستقل از تقویم سیستم و بر اساس تقویم استاندارد عمل می کند.**
## امکانات
- پشتیبانی از تقویم جلالی (فارسی)
- امکان انتخاب تاریخ سریع به صورت تعاملی
- امکان شخصی سازی اندازه کنترل، رنگهای پایه، فونت نمایش تاریخ به حروف.
- برگرداندن تاریخ انتخاب شده در قالب دو آبجکت DateTime و MD.PersianDateTime.
## نصب
  از طریق NuGet اقدام کنید:
```sh
dotnet add package PersianMonthView
```
  یا از طریق ویژوال استودیو:
  1.**Manage NuGet Packages**را باز کنید
  2.`PersianMonthView` را جستجو کنید
  3.**Install**را انتخاب کنید.
## نحوه استفاده:
```csharp
var persianDatePicker = new PersianMonthView();
Controls.Add(persianDatePicker);

// Get selected date
DateTime selectedDate = persianDatePicker.SelectedDate;
PersianDateTime persianSelectedDate = persianDatePicker.SelectedPersianDate;
```
  این کنترل رایگان و تحت لایسنس MIT می باشد.

  **برای کسب اطلاعات بیشتر جهت استفاده از امکانات آبجکت MD.PersianDateTime به [اینجا](https://github.com/Mds92/MD.PersianDateTime) مراجعه کنید**
***
  **با تشکر آقای [محمد دیان](https://github.com/Mds92)**
***
## لیست تغییرات:
- ver 1.0.0 
	نسخه اولیه
- ver 1.0.1 
-	پنهان سازی Tag به دلیل کاربرد منطقی
- ver 1.0.2 
	- به روز رسانی لایسنس و افزودن این فایل خلاصه
- ver 1.0.3 	
- رفع باگ مربوط به Tag که باعث عدم اضافه شدن کنترل می شد.
- ver 1.0.4-BetaTest
- رفع مشکل تغییر سایز کنترل هنگام اضافه کردن به گروپ باکس با فونت سایز متفاوت
- نسخه تست دنیای واقعی
- ver 1.0.5
- رفع مشکل عدم نمایش لیست روزها در انتخاب سریع تاریخ
***
**با من در تماس باشید:
[amin.shafeie@outlook.com](mailto:amin.shafeie@outlook.com)**
***
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


## License
This package is licensed under the **MIT License**.
**For more information on how to use PersianDateTime object features please refer [here](https://github.com/Mds92/MD.PersianDateTime)**

**Thanks to [Mohammad Dayyan](https://github.com/Mds92)**
***
**Contact me at: [amin.shafeie@outlook.com](mailto:amin.shafeie@outlook.com)**
***
