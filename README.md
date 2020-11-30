# Âm Lịch Việt Nam (Vietnam Lunar Day)

## Hướng dẫn (Tutorials)

### Cách chuyển đổi ngày dương sang ngày âm (How to convert solar day to lunar day)

Sử dụng phương thức `AmLich.convertSolar2Lunar` (AmLich.cs)

```
// Convert 14/1/1998 (January 14th 1998) to lunar day
int[] lunarDay = AmLich.convertSolar2Lunar(14, 1, 1998, 7.0)  // 7.0 is fixed timezone of Vietnam

// lunarDay = [17, 12, 1997, 0.0] => 14/1/1998 (solar) -> 17/12/1997 (lunar)
```

## Tham khảo (References)

- http://www.informatik.uni-leipzig.de/~duc/amlich/
- http://www.informatik.uni-leipzig.de/~duc/amlich/VietCalendar.java
