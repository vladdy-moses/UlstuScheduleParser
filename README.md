# Парсер расписания УлГТУ на C#

[![Build Status](https://travis-ci.org/vladdy-moses/UlstuScheduleParser.svg?branch=master)](https://travis-ci.org/vladdy-moses/UlstuScheduleParser)
[![Coverage Status](https://coveralls.io/repos/github/vladdy-moses/UlstuScheduleParser/badge.svg)](https://coveralls.io/github/vladdy-moses/UlstuScheduleParser)

Ещё один проект по парсингу расписания УлГТУ с официального сайта http://ulstu.ru/.

Особенности:
- написан на .NET Core;
- немного покрыт тестами;
- позволяет распарсить расписание с сайта или из предварительно сохранённой копии в файле.

В качестве примера работы был разработан генератор HTML-разметки для расписания конкретной аудитории.
Пример можно увидеть здесь: http://prntscr.com/mynwwd.

## Известные проблемы

1. Парсинг строки `пр.Начертательная геометрия и инженерная графика |Горшков Г М 6-418 |Рандин А В  |` создаёт только один элемент расписания, т.к. у Рандина не указана аудитория.
