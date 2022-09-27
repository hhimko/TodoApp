"use strict";
var resizeData = null;
function parseCSSNumericValue(value, unit) {
    var match = value.match("(-?\\d+)".concat(unit));
    if (!match)
        return null;
    var parsedValue = match[1];
    return Number(parsedValue);
}
function getScheduleTimeSpan(scheduleItem) {
    var timeSpan = Number(scheduleItem.style.getPropertyValue("--time-span"));
    if (timeSpan == 0 || isNaN(timeSpan))
        throw new Error("Schedule item's style was not instatiated with a '--time-span' value or the value was corrupted");
    return timeSpan;
}
function getScheduleRowHeight() {
    var cssValue = getComputedStyle(document.documentElement).getPropertyValue("--schedule-row-height");
    var rowHeight = parseCSSNumericValue(cssValue, "px");
    console.log(cssValue);
    if (!rowHeight || rowHeight == 0 || isNaN(rowHeight))
        throw new Error("Root element's style was not instatiated with a '--schedule-row-height' value or the value was corrupted");
    return rowHeight;
}
function getScheduleResizeMaxSpan(scheduleItem) {
    return 8;
}
function scheduleResize(e) {
    if (!resizeData)
        return scheduleResizeFinalize(e);
    var handleY = resizeData.resizeHandle.getBoundingClientRect().bottom;
    var mouseY = e.clientY;
    var spanElem = getScheduleTimeSpan(resizeData.scheduleItem);
    var rowHeight = getScheduleRowHeight();
    var spanDiff = Math.floor((mouseY - handleY) / rowHeight) + 1;
    if (spanDiff != 0) {
        var spanCurr = spanElem + spanDiff;
        if (spanCurr > 0 && spanCurr <= resizeData.maxTimeSpan)
            resizeData.scheduleItem.style.setProperty("--time-span", spanCurr.toString());
    }
}
function scheduleResizeFinalize(e) {
    if (e.target && e.target instanceof HTMLElement) {
        if (resizeData && e.target.closest(".schedule")) {
            // apply changes
        }
    }
    resizeData = null;
    window.removeEventListener("mousemove", scheduleResize);
    window.removeEventListener("mouseup", scheduleResizeFinalize);
}
function handleScheduleResizeMouseDown(e) {
    if (!(e.target && e.target instanceof HTMLElement))
        return;
    e.preventDefault();
    var resizeHandle = e.target;
    var scheduleItem = resizeHandle.closest(".schedule-item");
    if (!scheduleItem)
        throw new Error("Schedule item could not be found in handle's parent nodes by 'closest'");
    var initTimeSpan = getScheduleTimeSpan(scheduleItem);
    var maxTimeSpan = getScheduleResizeMaxSpan(scheduleItem);
    resizeData = { scheduleItem: scheduleItem, resizeHandle: resizeHandle, initTimeSpan: initTimeSpan, maxTimeSpan: maxTimeSpan };
    window.addEventListener("mousemove", scheduleResize);
    window.addEventListener("mouseup", scheduleResizeFinalize);
}
function handleScheduleMoveElemLoad(e) {
}
//# sourceMappingURL=schedule.js.map