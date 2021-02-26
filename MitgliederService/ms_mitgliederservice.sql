-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Feb 26, 2021 at 10:27 AM
-- Server version: 10.4.17-MariaDB
-- PHP Version: 7.2.34

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `ms_mitgliederservice`
--

-- --------------------------------------------------------

--
-- Table structure for table `fussballspieler`
--

CREATE TABLE `fussballspieler` (
  `id` int(33) NOT NULL,
  `Position` varchar(33) DEFAULT NULL,
  `Spieler_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `fussballspieler`
--

INSERT INTO `fussballspieler` (`id`, `Position`, `Spieler_ID`) VALUES
(4, 'Sturm', 4);

-- --------------------------------------------------------

--
-- Table structure for table `handballspieler`
--

CREATE TABLE `handballspieler` (
  `id` int(33) NOT NULL,
  `Position` varchar(33) DEFAULT NULL,
  `Spieler_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `handballspieler`
--

INSERT INTO `handballspieler` (`id`, `Position`, `Spieler_ID`) VALUES
(2, 'Front', 6);

-- --------------------------------------------------------

--
-- Table structure for table `mitglied`
--

CREATE TABLE `mitglied` (
  `id` int(33) NOT NULL,
  `Name` varchar(33) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `mitglied`
--

INSERT INTO `mitglied` (`id`, `Name`) VALUES
(6, 'Max Mustermann'),
(7, 'Lukas Test'),
(8, 'Tim Gabel'),
(9, 'Vanessa Jank'),
(10, 'Lucas Elm');

-- --------------------------------------------------------

--
-- Table structure for table `physiotherapeut`
--

CREATE TABLE `physiotherapeut` (
  `id` int(33) NOT NULL,
  `Mitglied_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `physiotherapeut`
--

INSERT INTO `physiotherapeut` (`id`, `Mitglied_ID`) VALUES
(3, 6);

-- --------------------------------------------------------

--
-- Table structure for table `spieler`
--

CREATE TABLE `spieler` (
  `id` int(33) NOT NULL,
  `AnzahlSpiele` int(11) DEFAULT NULL,
  `Sportart` varchar(33) DEFAULT NULL,
  `Mitglied_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `spieler`
--

INSERT INTO `spieler` (`id`, `AnzahlSpiele`, `Sportart`, `Mitglied_ID`) VALUES
(4, 10, 'Fussball', 8),
(5, 250, 'Tennis', 9),
(6, 3, 'Handball', 10);

-- --------------------------------------------------------

--
-- Table structure for table `tennisspieler`
--

CREATE TABLE `tennisspieler` (
  `id` int(33) NOT NULL,
  `JahreErfahrung` int(11) DEFAULT NULL,
  `Spieler_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tennisspieler`
--

INSERT INTO `tennisspieler` (`id`, `JahreErfahrung`, `Spieler_ID`) VALUES
(2, 500, 5);

-- --------------------------------------------------------

--
-- Table structure for table `trainer`
--

CREATE TABLE `trainer` (
  `id` int(33) NOT NULL,
  `Mitglied_ID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `trainer`
--

INSERT INTO `trainer` (`id`, `Mitglied_ID`) VALUES
(2, 7);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `fussballspieler`
--
ALTER TABLE `fussballspieler`
  ADD PRIMARY KEY (`id`),
  ADD KEY `Spieler_ID` (`Spieler_ID`);

--
-- Indexes for table `handballspieler`
--
ALTER TABLE `handballspieler`
  ADD PRIMARY KEY (`id`),
  ADD KEY `Spieler_ID` (`Spieler_ID`);

--
-- Indexes for table `mitglied`
--
ALTER TABLE `mitglied`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `physiotherapeut`
--
ALTER TABLE `physiotherapeut`
  ADD PRIMARY KEY (`id`),
  ADD KEY `Mitglied_ID` (`Mitglied_ID`);

--
-- Indexes for table `spieler`
--
ALTER TABLE `spieler`
  ADD PRIMARY KEY (`id`),
  ADD KEY `Mitglied_ID` (`Mitglied_ID`);

--
-- Indexes for table `tennisspieler`
--
ALTER TABLE `tennisspieler`
  ADD PRIMARY KEY (`id`),
  ADD KEY `Spieler_ID` (`Spieler_ID`);

--
-- Indexes for table `trainer`
--
ALTER TABLE `trainer`
  ADD PRIMARY KEY (`id`),
  ADD KEY `Mitglied_ID` (`Mitglied_ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `fussballspieler`
--
ALTER TABLE `fussballspieler`
  MODIFY `id` int(33) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;

--
-- AUTO_INCREMENT for table `handballspieler`
--
ALTER TABLE `handballspieler`
  MODIFY `id` int(33) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `mitglied`
--
ALTER TABLE `mitglied`
  MODIFY `id` int(33) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT for table `physiotherapeut`
--
ALTER TABLE `physiotherapeut`
  MODIFY `id` int(33) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT for table `spieler`
--
ALTER TABLE `spieler`
  MODIFY `id` int(33) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT for table `tennisspieler`
--
ALTER TABLE `tennisspieler`
  MODIFY `id` int(33) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT for table `trainer`
--
ALTER TABLE `trainer`
  MODIFY `id` int(33) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `fussballspieler`
--
ALTER TABLE `fussballspieler`
  ADD CONSTRAINT `fussballspieler_ibfk_1` FOREIGN KEY (`Spieler_ID`) REFERENCES `spieler` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `handballspieler`
--
ALTER TABLE `handballspieler`
  ADD CONSTRAINT `handballspieler_ibfk_1` FOREIGN KEY (`Spieler_ID`) REFERENCES `spieler` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `physiotherapeut`
--
ALTER TABLE `physiotherapeut`
  ADD CONSTRAINT `physiotherapeut_ibfk_1` FOREIGN KEY (`Mitglied_ID`) REFERENCES `mitglied` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `spieler`
--
ALTER TABLE `spieler`
  ADD CONSTRAINT `spieler_ibfk_1` FOREIGN KEY (`Mitglied_ID`) REFERENCES `mitglied` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `tennisspieler`
--
ALTER TABLE `tennisspieler`
  ADD CONSTRAINT `tennisspieler_ibfk_1` FOREIGN KEY (`Spieler_ID`) REFERENCES `spieler` (`id`) ON DELETE CASCADE;

--
-- Constraints for table `trainer`
--
ALTER TABLE `trainer`
  ADD CONSTRAINT `trainer_ibfk_1` FOREIGN KEY (`Mitglied_ID`) REFERENCES `mitglied` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
