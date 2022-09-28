"use strict";
var resizeData = null;
function clamp(value, min, max) {
    return Math.max(min, Math.min(max, value));
}
function parseCSSNumericValue(value, unit) {
    const match = value.match(`(-?\\d+)${unit}`);
    if (!match)
        return null;
    const [, parsedValue] = match;
    return Number(parsedValue);
}
function getScheduleTimeSpan(scheduleItem) {
    const timeSpan = Number(scheduleItem.style.getPropertyValue("--time-span"));
    if (timeSpan == 0 || isNaN(timeSpan))
        throw new Error("Schedule item's style was not instatiated with a '--time-span' value or the value was corrupted");
    return timeSpan;
}
function getScheduleRowHeight() {
    const cssValue = getComputedStyle(document.documentElement).getPropertyValue("--schedule-row-height");
    const rowHeight = parseCSSNumericValue(cssValue, "px");
    if (!rowHeight || rowHeight == 0 || isNaN(rowHeight))
        throw new Error("Root element's style was not instatiated with a '--schedule-row-height' value or the value was corrupted");
    return rowHeight;
}
function getScheduleRowTimeFromId(value) {
    const match = value.match(/s-(\d+)-(\d+)$/);
    if (match && match.length == 3) {
        const h = Number(match[1]);
        const q = Number(match[2]);
        if (!(Number.isNaN(h) && Number.isNaN(q)))
            return [h, q];
    }
    throw new Error(`Could not retrieve schedule row time from '${value}'`);
}
function getScheduleResizeMaxSpan(scheduleItem) {
    let itemIndex = -1;
    for (const c of scheduleItem.classList) {
        const match = c.match(/schedule-item-(\d+)$/);
        if (match && match.length == 2) {
            itemIndex = Number(match[1]);
            break;
        }
    }
    if (itemIndex < 0 || Number.isNaN(itemIndex))
        throw new Error("Schedule item is missing an indexed '.schedule-item-{index}' class");
    const itemRow = scheduleItem.closest(".schedule-right");
    if (!itemRow)
        throw new Error("Schedule item is missing a parent with class '.schedule-right'");
    const [h, q] = getScheduleRowTimeFromId(itemRow.id);
    const itemRowIndex = h * 4 + q;
    const nextItem = document.querySelector(`.schedule-item-${itemIndex + 1}`);
    if (nextItem) {
        const nextItemRow = nextItem.closest(".schedule-right");
        if (!nextItemRow)
            throw new Error("Schedule item is missing a parent with class '.schedule-right'");
        const [h, q] = getScheduleRowTimeFromId(nextItemRow.id);
        const nextItemRowIndex = h * 4 + q;
        return nextItemRowIndex - itemRowIndex;
    }
    return 0;
}
function scheduleResize(e) {
    if (!resizeData)
        return scheduleResizeFinalize(e);
    const handleY = resizeData.resizeHandle.getBoundingClientRect().bottom;
    const mouseY = e.clientY;
    const spanElem = getScheduleTimeSpan(resizeData.scheduleItem);
    const rowHeight = getScheduleRowHeight();
    const spanDiff = Math.floor((mouseY - handleY) / rowHeight) + 1;
    if (spanDiff != 0) {
        const spanCurr = clamp(spanElem + spanDiff, 1, resizeData.maxTimeSpan);
        resizeData.scheduleItem.style.setProperty("--time-span", spanCurr.toString());
    }
}
function scheduleResizeFinalize(e) {
    if (e.target && e.target instanceof HTMLElement) {
        if (resizeData && e.target.closest(".schedule")) {
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
    const resizeHandle = e.target;
    const scheduleItem = resizeHandle.closest(".schedule-item");
    if (!scheduleItem)
        throw new Error("Schedule item could not be found in handle's parent nodes by 'closest'");
    const initTimeSpan = getScheduleTimeSpan(scheduleItem);
    const maxTimeSpan = getScheduleResizeMaxSpan(scheduleItem);
    resizeData = { scheduleItem, resizeHandle, initTimeSpan, maxTimeSpan };
    window.addEventListener("mousemove", scheduleResize);
    window.addEventListener("mouseup", scheduleResizeFinalize);
}
function handleScheduleMoveElemLoad(e) {
}
//# sourceMappingURL=schedule.js.map