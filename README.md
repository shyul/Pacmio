# Pacmio data analysis lab project

Welcome to the pacmio, a lab project for testing the theories of using big data for decision making. To get the project going, I am using the financial data, like historical stock price, as the data sample. Such data is generally free and reflecting real-world cases. We can also use other types of data sets, as long as they are free, and without any legal complications.

I have re-initialized this repository to removed any possible foul language / personal data / conflicting data in the previous commit comments.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

* C# Programming Language and .Net Framework 4.8
* Interactive Brokers account and related market data subscriptions
* Interactive TWS Gateway 974.4 or above [Latest](https://www.interactivebrokers.com/en/index.php?f=16454)
* Microsoft Visual Studio Community [Download](https://visualstudio.microsoft.com/downloads/)

### Installing

* Download the code and open pacmio.sln with Visual Studio

## Methods / Workflow

* Download historical data - IB API
* Storage and caching historical data - Json and Binary Serializations
* Apply technical analysis algorithms
* Analysis visualization and human observation -> Mosaic and different view options
* Trading rule syntax -> yield "watchlist"
* Batch trials
* Trial result visualization
* Execute trading rules: paper account, production account. (Realtime data (Tick and L2) -> Trading Rule -> Order)
* Execute: Account, positions

## Modules (To be moved to Wiki)

* Symbol
* Bar and BarTable


```
Q: "Why don't you use Python?"
A: "Why do you ask that question?"
```

## Authors

* **Xu Li** - *Initial work* - [github](https://github.com/shyul), [LinkedIn](https://www.linkedin.com/in/shyul/)

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## License

This project is licensed under the GNU General Public License v3 - see the [LICENSE](LICENSE) file for details
