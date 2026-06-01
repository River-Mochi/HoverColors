// File: UI/src/localization.ts
// Purpose: In-city panel localization.
// CS2 exposes the active game locale through the built-in app.activeLocale binding.
// The C# LocaleXX classes localize Options UI; these JSON files localize the Cohtml panel/tooltips.

import React from "react";
import { bindValue, useValue } from "cs2/api";

import deDE from "../../L10n/lang/de-DE.json";
import enUS from "../../L10n/lang/en-US.json";
import esES from "../../L10n/lang/es-ES.json";
import frFR from "../../L10n/lang/fr-FR.json";
import itIT from "../../L10n/lang/it-IT.json";
import jaJP from "../../L10n/lang/ja-JP.json";
import koKR from "../../L10n/lang/ko-KR.json";
import plPL from "../../L10n/lang/pl-PL.json";
import ptBR from "../../L10n/lang/pt-BR.json";
import thTH from "../../L10n/lang/th-TH.json";
import trTR from "../../L10n/lang/tr-TR.json";
import viVN from "../../L10n/lang/vi-VN.json";
import zhHANS from "../../L10n/lang/zh-HANS.json";
import zhHANT from "../../L10n/lang/zh-HANT.json";

const activeLocale$ = bindValue<string>("app", "activeLocale", "en-US");

export type LocaleKey = keyof typeof enUS;
type LocaleDictionary = Partial<Record<LocaleKey, string>>;

const dictionaries: Record<string, LocaleDictionary> = {
    "de-de": deDE,
    "en-us": enUS,
    "es-es": esES,
    "fr-fr": frFR,
    "it-it": itIT,
    "ja-jp": jaJP,
    "ko-kr": koKR,
    "pl-pl": plPL,
    "pt-br": ptBR,
    "th": thTH,
    "th-th": thTH,
    "tr": trTR,
    "tr-tr": trTR,
    "vi": viVN,
    "vi-vn": viVN,
    "zh-hans": zhHANS,
    "zh-hant": zhHANT,
};

const normalizeLocaleId = (localeId: string | null | undefined) =>
    (localeId ?? "en-US").replace("_", "-").toLowerCase();

export const usePanelLocalization = () => {
    const activeLocale = useValue(activeLocale$);

    return React.useCallback((key: LocaleKey) => {
        const dictionary = dictionaries[normalizeLocaleId(activeLocale)] ?? enUS;
        return dictionary[key] ?? enUS[key] ?? key;
    }, [activeLocale]);
};
