// File: UI/src/panel/usePanelDrag.ts
// Keeps the GTL-anchored panel draggable while clamping it inside the game window.

import React from "react";

type PanelOffset = {
    x: number;
    y: number;
};

type PanelDragState = {
    pointerX: number;
    pointerY: number;
    originX: number;
    originY: number;
    originLeft: number;
    originTop: number;
    originWidth: number;
    originHeight: number;
};

export const usePanelDrag = () => {
    const [panelOffset, setPanelOffset] = React.useState<PanelOffset>({ x: 0, y: 0 });
    const [panelDragging, setPanelDragging] = React.useState(false);

    const panelElementRef = React.useRef<HTMLDivElement | null>(null);
    const panelDragFrameRef = React.useRef<number | null>(null);
    const panelDragPendingOffsetRef = React.useRef(panelOffset);
    const panelDragRef = React.useRef<PanelDragState | null>(null);

    React.useEffect(() => {
        if (!panelDragging) {
            return;
        }

        const onMove = (e: MouseEvent) => {
            const d = panelDragRef.current;
            if (d == null) {
                return;
            }

            const dx = e.clientX - d.pointerX;
            const dy = e.clientY - d.pointerY;
            let nx = d.originX + dx;
            let ny = d.originY + dy;
            const nl = d.originLeft + dx;
            const nt = d.originTop + dy;
            const nr = nl + d.originWidth;
            const nb = nt + d.originHeight;

            if (nl < 0) {
                nx -= nl;
            }
            if (nt < 0) {
                ny -= nt;
            }
            if (nr > window.innerWidth) {
                nx -= nr - window.innerWidth;
            }
            if (nb > window.innerHeight) {
                ny -= nb - window.innerHeight;
            }

            panelDragPendingOffsetRef.current = { x: nx, y: ny };
            if (panelDragFrameRef.current == null) {
                panelDragFrameRef.current = window.requestAnimationFrame(() => {
                    panelDragFrameRef.current = null;
                    setPanelOffset(panelDragPendingOffsetRef.current);
                });
            }
        };

        const onUp = () => {
            if (panelDragFrameRef.current != null) {
                window.cancelAnimationFrame(panelDragFrameRef.current);
                panelDragFrameRef.current = null;
            }
            panelDragRef.current = null;
            setPanelDragging(false);
            setPanelOffset(panelDragPendingOffsetRef.current);
        };

        window.addEventListener("mousemove", onMove);
        window.addEventListener("mouseup", onUp);
        return () => {
            window.removeEventListener("mousemove", onMove);
            window.removeEventListener("mouseup", onUp);
        };
    }, [panelDragging]);

    React.useEffect(() => () => {
        if (panelDragFrameRef.current != null) {
            window.cancelAnimationFrame(panelDragFrameRef.current);
            panelDragFrameRef.current = null;
        }
    }, []);

    const handlePanelDragStart = React.useCallback((e: React.MouseEvent<HTMLDivElement>) => {
        e.preventDefault();
        e.stopPropagation();

        const rect = panelElementRef.current?.getBoundingClientRect();
        panelDragPendingOffsetRef.current = panelOffset;
        panelDragRef.current = {
            pointerX: e.clientX,
            pointerY: e.clientY,
            originX: panelOffset.x,
            originY: panelOffset.y,
            originLeft: rect?.left ?? 0,
            originTop: rect?.top ?? 0,
            originWidth: rect?.width ?? 0,
            originHeight: rect?.height ?? 0,
        };
        setPanelDragging(true);
    }, [panelOffset]);

    return {
        panelOffset,
        panelDragging,
        panelElementRef,
        handlePanelDragStart,
    };
};
