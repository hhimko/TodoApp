type ResizeData = {
    scheduleItem: HTMLElement,
    resizeHandle: HTMLElement,
    initTimeSpan: number,
    maxTimeSpan: number
}


var resizeData: ResizeData | null = null

function clamp(value: number, min: number, max: number): number {
    return Math.max(min, Math.min(max, value))
}


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

    if (!rowHeight || rowHeight == 0 || isNaN(rowHeight))
        throw new Error("Root element's style was not instatiated with a '--schedule-row-height' value or the value was corrupted")

    return rowHeight
}

function getScheduleRowTimeFromId(value: string): [number, number] {
    const match = value.match(/s-(\d+)-(\d+)$/)
    if (match && match.length == 3) {
        const h = Number(match[1])
        const q = Number(match[2])

        if (!(Number.isNaN(h) && Number.isNaN(q)))
            return [h, q]
    }
    throw new Error(`Could not retrieve schedule row time from '${value}'`)
}

function getScheduleResizeMaxSpan(scheduleItem: HTMLElement): number {
    let itemIndex = -1
    for (const c of scheduleItem.classList) {
        const match = c.match(/schedule-item-(\d+)$/)
        if (match && match.length == 2) {
            itemIndex = Number(match[1])
            break
        }
    }

    if (itemIndex < 0 || Number.isNaN(itemIndex))
        throw new Error("Schedule item is missing an indexed '.schedule-item-{index}' class")

    const itemRow = scheduleItem.closest<HTMLElement>(".schedule-right")
    if (!itemRow)
        throw new Error("Schedule item is missing a parent with class '.schedule-right'")

    const [h, q] = getScheduleRowTimeFromId(itemRow.id)
    const itemRowIndex = h * 4 + q

    const nextItem = document.querySelector(`.schedule-item-${itemIndex + 1}`)
    if (nextItem) {
        const nextItemRow = nextItem.closest<HTMLElement>(".schedule-right")
        if (!nextItemRow)
            throw new Error("Schedule item is missing a parent with class '.schedule-right'")

        const [h, q] = getScheduleRowTimeFromId(nextItemRow.id)
        const nextItemRowIndex = h * 4 + q

        return nextItemRowIndex - itemRowIndex
    }

    /*const itemRow = scheduleItem.closest<HTMLElement>(".schedule-right")
    if (!itemRow)
        throw new Error("Schedule item is missing a parent with class '.schedule-right'")

    const scheduleItems = Array.from(document.querySelectorAll<HTMLElement>(".schedule-item"))
    const resizedScheduleItem

    const scheduleItemsRows = scheduleItems.s(item => item.closest(""))
    const succeedingItems = Array.from(scheduleItems).filter(item => item.id > rowCurr.id)

    console.log(rowIter)*/

    return 0
}

function scheduleResize(e: MouseEvent): void {
    if (!resizeData) return scheduleResizeFinalize(e)

    const handleY = resizeData.resizeHandle.getBoundingClientRect().bottom
    const mouseY = e.clientY

    const spanElem = getScheduleTimeSpan(resizeData.scheduleItem)
    const rowHeight = getScheduleRowHeight()

    const spanDiff = Math.floor((mouseY - handleY) / rowHeight) + 1
    if (spanDiff != 0) {
        const spanCurr = clamp(spanElem + spanDiff, 1, resizeData.maxTimeSpan)
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
