module.exports = {
  root: true,
  env: {
    browser: true,
    es6: true,
    'jest/globals': true,
    node: true
  },
  extends: [
    '@nuxtjs/eslint-config-typescript',
    'plugin:nuxt/recommended',
    'eslint:recommended',
    'plugin:vue/essential',
    '@vue/prettier'
  ],
  parserOptions: {
    ecmaVersion: 2018,
    sourceType: 'module',
  },
  plugins: [
    'jest',
    'vue'
  ],
  rules: {
    'no-console': 'off',
    'vue/multiline-html-element-content-newline': [
      2,
      {
        ignoreWhenEmpty: true,
        ignores: ['pre', 'textarea'],
        allowEmptyLines: false,
      },
    ],
  }
}
