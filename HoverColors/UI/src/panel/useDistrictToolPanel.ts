// File: UI/src/panel/useDistrictToolPanel.ts
// Purpose: Opens the vanilla Areas > District tool after the District color menu is used.
// Keep this separate from the visual District menu so the panel component stays readable.

import React from "react";
import { toolbar } from "cs2/bindings";
import { useMapValue, useValue } from "cs2/api";
import {
  AREA_MENU_NAME_TOKENS,
  DISTRICT_AREA_NAME_TOKENS,
  sameEntity,
} from "./MochiPanelBindings";

export const useDistrictToolPanel = () => {
  const toolbarGroups = useValue(toolbar.toolbarGroups$);
  const selectedAssetMenu = useValue(toolbar.selectedAssetMenu$);
  const selectedAssetCategory = useValue(toolbar.selectedAssetCategory$);
  const selectedAsset = useValue(toolbar.selectedAsset$);

  const [pendingDistrictToolOpen, setPendingDistrictToolOpen] = React.useState(false);
  const areaPanelOpenTimerRef = React.useRef<number | null>(null);
  const districtToolOpenTimeoutRef = React.useRef<number | null>(null);
  const districtToolSelectRetryRef = React.useRef<number | null>(null);

  const normalizeToolbarName = React.useCallback((value: string) => value.toUpperCase(), []);

  const areasMenu = React.useMemo(() => (
    toolbarGroups
      ?.flatMap(group => group.children ?? [])
      .find(item =>
        item.type === toolbar.ToolbarItemType.menu
        && AREA_MENU_NAME_TOKENS.some(token => normalizeToolbarName(item.name).includes(token)))
  ), [normalizeToolbarName, toolbarGroups]);

  const areasCategories = useMapValue(toolbar.assetCategories$, areasMenu?.entity);

  const districtCategory = React.useMemo(() => (
    areasCategories
      ?.find(item => DISTRICT_AREA_NAME_TOKENS.some(token => normalizeToolbarName(item.name).includes(token)))
  ), [areasCategories, normalizeToolbarName]);

  const districtAssets = useMapValue(toolbar.assets$, districtCategory?.entity);

  const districtAsset = React.useMemo(() => (
    districtAssets
      ?.find(item => DISTRICT_AREA_NAME_TOKENS.some(token => normalizeToolbarName(item.name).includes(token)))
    ?? districtAssets?.[0]
  ), [districtAssets, normalizeToolbarName]);

  // Important:
  // Repeated District button/swatch clicks must not keep reopening the vanilla Areas panel.
  // If the game is already on Areas > District, the mini panel can open/close by itself.
  const isDistrictToolSelected = React.useMemo(() => (
    areasMenu != null
    && districtCategory != null
    && districtAsset != null
    && sameEntity(selectedAssetMenu, areasMenu.entity)
    && sameEntity(selectedAssetCategory, districtCategory.entity)
    && sameEntity(selectedAsset, districtAsset.entity)
  ), [
    areasMenu,
    districtAsset,
    districtCategory,
    selectedAsset,
    selectedAssetCategory,
    selectedAssetMenu,
  ]);

  // Areas panel is already open if the selected vanilla toolbar menu is Areas.
  // We still may need to select the District category once, but we should not clear/reopen.
  const isAreasMenuOpen = React.useMemo(() => (
    areasMenu != null
    && sameEntity(selectedAssetMenu, areasMenu.entity)
  ), [
    areasMenu,
    selectedAssetMenu,
  ]);

  const isDistrictCategoryOpen = React.useMemo(() => (
    isAreasMenuOpen
    && districtCategory != null
    && sameEntity(selectedAssetCategory, districtCategory.entity)
  ), [
    districtCategory,
    isAreasMenuOpen,
    selectedAssetCategory,
  ]);

  const clearOpenTimers = React.useCallback(() => {
    if (areaPanelOpenTimerRef.current != null) {
      clearTimeout(areaPanelOpenTimerRef.current);
      areaPanelOpenTimerRef.current = null;
    }

    if (districtToolOpenTimeoutRef.current != null) {
      clearTimeout(districtToolOpenTimeoutRef.current);
      districtToolOpenTimeoutRef.current = null;
    }

    if (districtToolSelectRetryRef.current != null) {
      clearTimeout(districtToolSelectRetryRef.current);
      districtToolSelectRetryRef.current = null;
    }
  }, []);

  React.useEffect(() => clearOpenTimers, [clearOpenTimers]);

  React.useEffect(() => {
    if (!pendingDistrictToolOpen) {
      return;
    }

    if (areasMenu == null) {
      setPendingDistrictToolOpen(false);
      return;
    }

    if (!sameEntity(selectedAssetMenu, areasMenu.entity)) {
      toolbar.selectAssetMenu(areasMenu.entity);
      return;
    }

    if (districtCategory == null) {
      return;
    }

    if (!sameEntity(selectedAssetCategory, districtCategory.entity)) {
      toolbar.selectAssetCategory(districtCategory.entity);
      return;
    }

    if (districtAsset == null) {
      return;
    }

    if (sameEntity(selectedAsset, districtAsset.entity)) {
      // Already on Areas > District. Do not reselect or flash the vanilla panel.
      clearOpenTimers();
      setPendingDistrictToolOpen(false);
      return;
    }

    // Select District without clearing first. Clearing causes the vanilla Areas panel flash.
    toolbar.selectAssetMenu(areasMenu.entity);
    toolbar.selectAssetCategory(districtCategory.entity);
    toolbar.selectAsset(districtAsset.entity, true);

    if (districtToolSelectRetryRef.current != null) {
      clearTimeout(districtToolSelectRetryRef.current);
    }

    const areasMenuEntity = areasMenu.entity;
    const districtCategoryEntity = districtCategory.entity;
    const districtEntity = districtAsset.entity;

    // Vanilla can restore the previous subtool, so retry once.
    // Do not call clearAssetSelection here; it makes the game Areas panel visibly flash.
    districtToolSelectRetryRef.current = window.setTimeout(() => {
      toolbar.selectAssetMenu(areasMenuEntity);
      toolbar.selectAssetCategory(districtCategoryEntity);
      toolbar.selectAsset(districtEntity, true);
      districtToolSelectRetryRef.current = null;
    }, 250);

    if (districtToolOpenTimeoutRef.current != null) {
      clearTimeout(districtToolOpenTimeoutRef.current);
      districtToolOpenTimeoutRef.current = null;
    }

    setPendingDistrictToolOpen(false);
  }, [
    areasMenu,
    clearOpenTimers,
    districtAsset,
    districtCategory,
    pendingDistrictToolOpen,
    selectedAsset,
    selectedAssetCategory,
    selectedAssetMenu,
  ]);

  const openAreasToolPanel = React.useCallback(() => {
    // Already on Areas > District: do nothing. This prevents repeated flashing.
    if (isDistrictToolSelected) {
      clearOpenTimers();
      setPendingDistrictToolOpen(false);
      return;
    }

    // Areas > District category is already open. Do not clear/reopen the panel.
    // The mini panel and ColorField picker can work without forcing vanilla selection again.
    if (isDistrictCategoryOpen) {
      clearOpenTimers();
      setPendingDistrictToolOpen(false);
      return;
    }

    clearOpenTimers();

    // Defer until after the picker/menu click finishes; toolbar bindings arrive in steps.
    areaPanelOpenTimerRef.current = window.setTimeout(() => {
      // Do not clear asset selection here. It causes the Areas panel to visibly shrink/flash.
      setPendingDistrictToolOpen(true);
      areaPanelOpenTimerRef.current = null;
    }, 80);

    // Bail out if the expected toolbar data never arrives.
    districtToolOpenTimeoutRef.current = window.setTimeout(() => {
      setPendingDistrictToolOpen(false);
      districtToolOpenTimeoutRef.current = null;
    }, 1500);
  }, [
    clearOpenTimers,
    isDistrictCategoryOpen,
    isDistrictToolSelected,
  ]);

  return { openAreasToolPanel };
};
