import { createI18n } from 'vue-i18n'
import de from './locales/de'
import en from './locales/en'

export const i18n = createI18n({
  legacy: false,
  locale: 'de',
  fallbackLocale: 'en',
  messages: { de, en },
})
