# Парсер расписания УлГТУ на C#

[![Build Status](https://travis-ci.org/vladdy-moses/UlstuScheduleParser.svg?branch=master)](https://travis-ci.org/vladdy-moses/UlstuScheduleParser)
[![Coverage Status](https://coveralls.io/repos/github/vladdy-moses/UlstuScheduleParser/badge.svg?branch=master)](https://coveralls.io/github/vladdy-moses/UlstuScheduleParser?branch=master)

Ещё один проект по парсингу расписания УлГТУ с официального сайта http://ulstu.ru/.

Особенности:
- написан на .NET Standard 2.0;
- покрыт тестами;
- позволяет распарсить расписание с сайта или из предварительно сохранённой копии в файле.

## Примеры использования

Примеры использования библиотеки см. в проекте _UlstuScheduleParser.Demo_.

Одним из примеров является генератор HTML-разметки для расписания конкретной аудитории.
Скриншот: http://prntscr.com/mynwwd.

## Известные проблемы

1. Парсинг строки `пр.Начертательная геометрия и инженерная графика |Горшков Г М 6-418 |Рандин А В  |` создаёт только один элемент расписания, т.к. у Рандина не указана аудитория.
