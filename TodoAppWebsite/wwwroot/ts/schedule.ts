type ResizeData = {
    scheduleItem: HTMLElement,
    resizeHandle: HTMLElement,
    initTimeSpan: number,
    maxTimeSpan: number
}


var resizeData: ResizeData | null = null


function parseCSSNumericValue(value: string, unit: string): number | null {
    const match = value.match(`(-?\\d+)${unit}`)
    if (!match)
        return null

    const [, parsedValue] = match
    return Number(parsedValue)
}

function getScheduleTimeSpan(scheduleItem: HTMLElement): number {
    const timeSpan = Number(scheduleItem.style.getPropertyValue("--time-span"))
    if (timeSpan == 0 || isNaN(timeSpan))
        throw new Error("Schedule item's style was not instatiated with a '--time-span' value or the value was corrupted")

    return timeSpan
}

function getScheduleRowHeight(): number {
    const cssValue = getComputedStyle(document.documentElement).getPropertyValue("--schedule-row-height")
    const rowHeight = parseCSSNumericValue(cssValue, "px")
    console.log(cssValue)
    if (!rowHeight || rowHeight == 0 || isNaN(rowHeight))
        throw new Error("Root element's style was not instatiated with a '--schedule-row-height' value or the value was corrupted")

    return rowHeight
}

function getScheduleResizeMaxSpan(scheduleItem: HTMLElement): number {
    return 8
}

function scheduleResize(e: MouseEvent): void {
    if (!resizeData) return scheduleResizeFinalize(e)

    const handleY = resizeData.resizeHandle.getBoundingClientRect().bottom
    const mouseY = e.clientY

    const spanElem = getScheduleTimeSpan(resizeData.scheduleItem)
    const rowHeight = getScheduleRowHeight()

    const spanDiff = Math.floor((mouseY - handleY) / rowHeight) + 1
    if (spanDiff != 0) {
        const spanCurr = spanElem + spanDiff

        if (spanCurr > 0 && spanCurr <= resizeData.maxTimeSpan)
            resizeData.scheduleItem.style.setProperty("--time-span", spanCurr.toString())
    }
}

function scheduleResizeFinalize(e: MouseEvent): void {
    if (e.target && e.target instanceof HTMLElement) {
        if (resizeData && e.target.closest(".schedule")) {
            // apply changes
        }
    }

    resizeData = null
    window.removeEventListener("mousemove", scheduleResize)
    window.removeEventListener("mouseup", scheduleResizeFinalize)
}

function handleScheduleResizeMouseDown(e: MouseEvent): void {
    if (!(e.target && e.target instanceof HTMLElement)) return
    e.preventDefault()

    const resizeHandle = e.target
    const scheduleItem = resizeHandle.closest<HTMLElement>(".schedule-item")
    if (!scheduleItem)
        throw new Error("Schedule item could not be found in handle's parent nodes by 'closest'")

    const initTimeSpan = getScheduleTimeSpan(scheduleItem)
    const maxTimeSpan = getScheduleResizeMaxSpan(scheduleItem)

    resizeData = { scheduleItem, resizeHandle, initTimeSpan, maxTimeSpan }

    window.addEventListener("mousemove", scheduleResize)
    window.addEventListener("mouseup", scheduleResizeFinalize)
}

function handleScheduleMoveElemLoad(e: MouseEvent): void {

}
