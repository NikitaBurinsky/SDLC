# Проект «NutriTrack» (Трекер-калькулятор питания)

## Автор
- **Студент:** Буринский Н.А.
- **Группа:** 450504
- **Дисциплина:** Жизненный цикл разработки программного обеспечения (ЖЦРПО)
- **Учреждение:** БГУИР, Факультет компьютерных систем и сетей, Кафедра ЭВМ
- **Преподаватель:** Внук О. М.

---

## Описание проекта
«NutriTrack» — кроссплатформенный веб-сервис для комплексного учета суточного рациона питания, расчета баланса калорий и макронутриентов (КБЖУ), интеграции с международной базой продуктов Open Food Facts и точного расчета сложных кулинарных рецептов с учетом уварки/ужарки (термической обработки).

### Технологический стек:
- **Backend:** C# / ASP.NET Core Web API (.NET 8/9), Entity Framework Core
- **Frontend:** TypeScript, React (Vite), Tailwind CSS
- **База данных:** PostgreSQL
- **Внешние API:** Open Food Facts REST API
- **Моделирование требований:** OMG SysML v2, Markdown

---

## Структура репозитория для Лабораторной работы №3:
- [`srs/srs-nutritrack.md`](srs/srs-nutritrack.md) — Спецификация требований к ПО (SRS) по стандарту ISO/IEC/IEEE 29148.
- [`srs/sysml/`](srs/sysml/) — Исходные текстовые спецификации на языке SysML v2:
  - `nutritrack_requirements.sysml` — иерархия требований.
  - `nutritrack_usecases.sysml` — модель прецедентов и акторов.
  - `nutritrack_gui_mockups.sysml` — структура пользовательского интерфейса.
- [`srs/images/`](srs/images/) — Графические артефакты:
  - `sysml_requirements_diagram.png` — диаграмма требований SysML v2.
  - `sysml_usecase_diagram.png` — диаграмма прецедентов SysML v2.
  - `mockup_main_dashboard.png` — мокап главного экрана дневника питания.
  - `mockup_add_food_dialog.png` — мокап добавления продукта через Open Food Facts.
  - `mockup_recipe_calculator.png` — мокап калькулятора составных блюд с уваркой.
