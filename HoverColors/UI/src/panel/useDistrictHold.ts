// File: UI/src/panel/useDistrictHold.ts
// Purpose: Quick-click/hold behavior for the bottom District color button.
//
// Quick click opens the District mini menu and also nudges the vanilla Areas tool panel open.
// Hold resets District colors to vanilla. The hold progress is used by the fill-sweep bar.

import React from "react";
import { DISTRICT_RESET_HOLD_MS } from "./MochiPanelBindings";

interface UseDistrictHoldOptions {
    onReset: () => void;
    onQuickClick: () => void;
}

export const useDistrictHold = ({ onReset, onQuickClick }: UseDistrictHoldOptions) => {
    const [districtHoldProgress, setDistrictHoldProgress] = React.useState(0);
    const districtHoldTimerRef = React.useRef<number | null>(null);
    const districtHoldStartRef = React.useRef<number>(0);
    const districtHoldRafRef = React.useRef<number | null>(null);
    const districtHoldCompletedRef = React.useRef(false);

    const cancelDistrictHold = React.useCallback(() => {
        if (districtHoldTimerRef.current != null) {
            clearTimeout(districtHoldTimerRef.current);
            districtHoldTimerRef.current = null;
        }
        if (districtHoldRafRef.current != null) {
            cancelAnimationFrame(districtHoldRafRef.current);
            districtHoldRafRef.current = null;
        }
        districtHoldCompletedRef.current = false;
        setDistrictHoldProgress(0);
    }, []);

    const handleDistrictMouseDownCapture = React.useCallback((event: React.MouseEvent<HTMLDivElement>) => {
        if (event.button !== 0) {
            return;
        }

        cancelDistrictHold();
        districtHoldCompletedRef.current = false;
        districtHoldStartRef.current = performance.now();
        setDistrictHoldProgress(0);

        const sweepDelay = 150;
        const tick = () => {
            const elapsed = performance.now() - districtHoldStartRef.current;
            if (elapsed >= sweepDelay) {
                const progress = Math.min((elapsed - sweepDelay) / (DISTRICT_RESET_HOLD_MS - sweepDelay), 1);
                setDistrictHoldProgress(progress);
                if (progress < 1) {
                    districtHoldRafRef.current = requestAnimationFrame(tick);
                }
            } else {
                districtHoldRafRef.current = requestAnimationFrame(tick);
            }
        };

        districtHoldRafRef.current = requestAnimationFrame(tick);
        districtHoldTimerRef.current = window.setTimeout(() => {
            districtHoldTimerRef.current = null;
            if (districtHoldRafRef.current != null) {
                cancelAnimationFrame(districtHoldRafRef.current);
                districtHoldRafRef.current = null;
            }
            districtHoldCompletedRef.current = true;
            onReset();
            setDistrictHoldProgress(0);
        }, DISTRICT_RESET_HOLD_MS);
    }, [cancelDistrictHold, onReset]);

    const handleDistrictMouseUpCapture = React.useCallback(() => {
        if (!districtHoldCompletedRef.current) {
            cancelDistrictHold();
        }
    }, [cancelDistrictHold]);

    const handleDistrictClickCapture = React.useCallback((event: React.MouseEvent<HTMLDivElement>) => {
        event.preventDefault();
        event.stopPropagation();

        if (!districtHoldCompletedRef.current) {
            onQuickClick();
            return;
        }

        districtHoldCompletedRef.current = false;
    }, [onQuickClick]);

    React.useEffect(() => () => {
        if (districtHoldTimerRef.current != null) {
            clearTimeout(districtHoldTimerRef.current);
            districtHoldTimerRef.current = null;
        }
        if (districtHoldRafRef.current != null) {
            cancelAnimationFrame(districtHoldRafRef.current);
            districtHoldRafRef.current = null;
        }
    }, []);

    return {
        districtHoldProgress,
        cancelDistrictHold,
        handleDistrictMouseDownCapture,
        handleDistrictMouseUpCapture,
        handleDistrictClickCapture,
    };
};
