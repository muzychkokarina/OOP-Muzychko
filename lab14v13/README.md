# Лабораторна робота №14

**Тема:** Аналіз SRP/OCP у відкритих репозиторіях.  
**Студентка:** Музичко Карина  
**Група / Варіант:** Варіант 13  
**Навчальний заклад:** Рівненський фаховий коледж інформаційних технологій  
**Інструменти:** GitHub, Visual Studio Code, Markdown  

---

## Мета роботи
Дослідити практичне застосування принципів єдиної відповідальності (**SRP — Single Responsibility Principle**) та відкритості/закритості (**OCP — Open/Closed Principle**) у реальних open-source проєктах на мові C#, проаналізувати їхній вплив на підтримку коду та запропонувати рефакторинг для виявленого порушення.

---

## Обрані open-source репозиторії

Для аналізу обрано два популярні C#-репозиторії з принципово різних предметних областей:
1. **eShopOnWeb** (`dotnet-architecture/eShopOnWeb`) — еталонний вебзастосунок електронної комерції (E-Commerce) на ASP.NET Core.  
   Посилання: https://github.com/dotnet-architecture/eShopOnWeb
2. **NLog** (`NLog/NLog`) — бібліотека для гнучкого логування подій у застосунках .NET.  
   Посилання: https://github.com/NLog/NLog

---

## Практичний аналіз SRP та OCP

### 1. Два приклади вдалого застосування SRP

#### Приклад 1.1: `EmailSender` у репозиторії `eShopOnWeb`
* **Файл:** `src/Infrastructure/Services/EmailSender.cs`  
* **Посилання:** https://github.com/dotnet-architecture/eShopOnWeb/blob/main/src/Infrastructure/Services/EmailSender.cs (рядки 1–28)
* **Де видно принцип:** Клас `EmailSender` реалізує інтерфейс `IEmailSender` і виконує виключно одну задачу — відправлення електронного листа через SMTP-клієнт (`SmtpClient`). Він не формує HTML-шаблони замовлень, не перевіряє бізнес-правила користувача і не звертається до бази даних.
* **Переваги для підтримки:** Зміна поштового провайдера (наприклад, перехід зі SMTP на SendGrid або Amazon SES) локалізується лише в цьому класі або новій реалізації `IEmailSender` без ризику зламати сервіси замовлень чи авторизації.
* **Що зміниться при додаванні нової функції:** Якщо знадобиться додавати вкладення (PDF-чек) до листа, достатньо розширити контракт відправки повідомлення, не зачіпаючи бізнес-логіку оформлення замовлення.

#### Приклад 1.2: `JsonLayout` у репозиторії `NLog`
* **Файл:** `src/NLog/Layouts/JsonLayout.cs`  
* **Посилання:** https://github.com/NLog/NLog/blob/dev/src/NLog/Layouts/JsonLayout.cs (рядки 40–185)
* **Де видно принцип:** Клас відповідає суто за серіалізацію структурованої події логування (`LogEventInfo`) у формат JSON згідно із заданими атрибутами (`JsonAttribute`). Він нічого не знає про те, куди саме буде записаний цей рядок (у файл, консоль чи мережевий сокет) — цим займаються класи `Target`.
* **Переваги для підтримки:** Розділення форматування (`Layout`) і транспорту (`Target`) дозволяє тестувати коректність генерації JSON ізольовано, без створення тимчасових файлів чи підняття мережевих з'єднань.
* **Що зміниться при додаванні нової функції:** При додаванні нового правила екранування спецсимволів у JSON змінюється лише `JsonLayout`, а всі наявні цілі запису (`FileTarget`, `NetworkTarget`) автоматично отримують оновлений формат.

---

### 2. Два приклади порушення SRP

#### Приклад 2.1: `OrderService` у репозиторії `eShopOnWeb`
* **Файл:** `src/ApplicationCore/Services/OrderService.cs`  
* **Посилання:** https://github.com/dotnet-architecture/eShopOnWeb/blob/main/src/ApplicationCore/Services/OrderService.cs (рядки 27–66)
* **Де видно порушення:** У методі `CreateOrderAsync` поєднано одразу кілька відповідальностей:
  1. Отримання кошика користувача з репозиторію (`_basketRepository`).
  2. Отримання даних про товари з каталогу та мапінг позицій замовлення (`_itemRepository`).
  3. Побудова URL-адрес зображень через інфраструктурний компонент `_uriComposer` (презентаційна/інфраструктурна логіка всередині доменного сервісу).
  4. Створення та збереження агрегату `Order` у БД (`_orderRepository`).
* **Ризики для підтримки:** Клас має кілька причин для змін: зміна правил формування URL картинок, зміна логіки розрахунку вартості замовлення або зміна структури кошика змушують редагувати один і той самий файл.
* **Що зміниться при додаванні нової функції:** Додавання знижок/промокодів або резервування товару на складі призведе до ще більшого розростання методу `CreateOrderAsync` та ускладнення модульного тестування (доведеться мокати 4+ залежностей).

#### Приклад 2.2: `LogFactory` у репозиторії `NLog`
* **Файл:** `src/NLog/LogFactory.cs`  
* **Посилання:** https://github.com/NLog/NLog/blob/dev/src/NLog/LogFactory.cs (рядки 55–450+)
* **Де видно порушення:** Клас `LogFactory` виступає як "God Object" (божественний об'єкт): він одночасно керує життєвим циклом фабрики логерів, кешуванням екземплярів `Logger`, завантаженням і перезавантаженням конфігурації (`LoggingConfiguration`), глобальними налаштуваннями обробки винятків (`ThrowExceptions`) та сповіщенням підписників про зміну конфігурації.
* **Ризики для підтримки:** Висока зв'язаність (high coupling) ускладнює безпечне внесення правок, оскільки зміна механізму кешування логерів може непередбачувано вплинути на процес гарячого перезавантаження конфігурації.
* **Що зміниться при додаванні нової функції:** Додавання нового джерела конфігурації або політики блокування потоків вимагатиме втручання у великий клас із десятками внутрішніх полів та станів.

---

### 3. Два приклади розширення за OCP

#### Приклад 3.1: Пайплайн специфікацій `ISpecification<T>` (`Ardalis.Specification`) у `eShopOnWeb`
* **Файли:**  
  - `src/ApplicationCore/Specifications/CatalogFilterPaginatedSpecification.cs`  
  - `src/ApplicationCore/Specifications/CustomerOrdersWithItemsSpecification.cs`  
* **Посилання:** https://github.com/dotnet-architecture/eShopOnWeb/tree/main/src/ApplicationCore/Specifications (рядки 1–25 у кожному файлі)
* **Де видно принцип:** Узагальнений репозиторій `IRepository<T>` залишається **закритим для модифікації** — у нього не додаються методи на кшталт `GetOrdersByCustomerAndDate()` або `GetProductsByBrandAndType()`. Натомість система **відкрита для розширення** через створення нових класів-специфікацій, які інкапсулюють критерії фільтрації, сортування та `Include`.
* **Переваги для підтримки:** Код репозиторію не розростається з появою нових бізнес-запитів, а кожен запит ізольований у власному невеликому класі.
* **Що зміниться при додаванні нової функції:** Для фільтрації товарів за діапазоном цін достатньо створити новий клас `CatalogPriceRangeSpecification`, не змінюючи жодного рядка в `EfRepository<T>`.

#### Приклад 3.2: Ієрархія цілей логування `Target` / `TargetWithLayout` у `NLog`
* **Файли:**  
  - Базова абстракція: `src/NLog/Targets/Target.cs` (рядки 50–120)  
  - Конкретні розширення: `src/NLog/Targets/ConsoleTarget.cs`, `src/NLog/Targets/FileTarget.cs`  
* **Посилання:** https://github.com/NLog/NLog/blob/dev/src/NLog/Targets/Target.cs
* **Де видно принцип:** Ядро маршрутизації логів працює з базовим абстрактним класом `Target` і викликає віртуальний метод `Write(LogEventInfo logEvent)`. Підтримка нових місць призначення логів реалізується через успадкування від `Target` або `TargetWithLayout` та перевизначення методу `Write`.
* **Переваги для підтримки:** Ядро `NLog` не містить громіздких конструкцій `switch (targetType)` і не потребує перекомпіляції при створенні кастомних плагінів запису логів.
* **Що зміниться при додаванні нової функції:** Для відправки логів у Telegram або Elasticsearch розробник створює власний клас `TelegramTarget : TargetWithLayout` і реєструє його через конфігурацію без зміни вихідного коду бібліотеки `NLog`.

---

## Пропозиція рефакторингу (для `OrderService` з `eShopOnWeb`)

Найбільш показовим для рефакторингу є `OrderService.CreateOrderAsync`, оскільки бізнес-сервіс створення замовлення залежить від інфраструктурного формування URI зображень (`IUriComposer`) та безпосередньо виконує перетворення кошика у позиції замовлення.

### Проблема поточного коду:
`OrderService` знає, як формувати посилання на картинки товарів (`_uriComposer.ComposePicUri(...)`) і як конструювати `OrderItem` з `BasketItem`.

### Рішення:
Виділити окремий мапер/фабрику `IOrderItemsFactory` (дотримання **SRP**), яка відповідатиме за перетворення елементів кошика та каталогу у позиції замовлення `OrderItem`.

```csharp
// 1. Нова вузькоспеціалізована абстракція (SRP)
public interface IOrderItemsFactory
{
    List<OrderItem> CreateOrderItems(Basket basket, IReadOnlyList<CatalogItem> catalogItems);
}

public class OrderItemsFactory : IOrderItemsFactory
{
    private readonly IUriComposer _uriComposer;

    public OrderItemsFactory(IUriComposer uriComposer)
    {
        _uriComposer = uriComposer;
    }

    public List<OrderItem> CreateOrderItems(Basket basket, IReadOnlyList<CatalogItem> catalogItems)
    {
        return basket.Items.Select(basketItem =>
        {
            var catalogItem = catalogItems.First(c => c.Id == basketItem.CatalogItemId);
            var itemOrdered = new CatalogItemOrdered(
                catalogItem.Id,
                catalogItem.Name,
                _uriComposer.ComposePicUri(catalogItem.PictureUri));

            return new OrderItem(itemOrdered, basketItem.UnitPrice, basketItem.Quantity);
        }).ToList();
    }
}

// 2. Спрощений OrderService, який координує лише збереження замовлення
public class OrderService : IOrderService
{
    private readonly IRepository<Order> _orderRepository;
    private readonly IRepository<Basket> _basketRepository;
    private readonly IRepository<CatalogItem> _itemRepository;
    private readonly IOrderItemsFactory _orderItemsFactory;

    public OrderService(
        IRepository<Basket> basketRepository,
        IRepository<CatalogItem> itemRepository,
        IRepository<Order> orderRepository,
        IOrderItemsFactory orderItemsFactory)
    {
        _basketRepository = basketRepository;
        _itemRepository = itemRepository;
        _orderRepository = orderRepository;
        _orderItemsFactory = orderItemsFactory;
    }

    public async Task CreateOrderAsync(int basketId, Address shippingAddress)
    {
        var basketSpec = new BasketWithItemsSpecification(basketId);
        var basket = await _basketRepository.FirstOrDefaultAsync(basketSpec);
        Guard.Against.Null(basket, nameof(basket));

        var catalogItemsSpecification = new CatalogItemsSpecification(
            basket.Items.Select(item => item.CatalogItemId).ToArray());
        var catalogItems = await _itemRepository.ListAsync(catalogItemsSpecification);

        var items = _orderItemsFactory.CreateOrderItems(basket, catalogItems);
        var order = new Order(basket.BuyerId, shippingAddress, items);

        await _orderRepository.AddAsync(order);
    }
}