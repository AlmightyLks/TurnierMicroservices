-- phpMyAdmin SQL Dump
-- version 5.0.4
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 01, 2021 at 07:20 PM
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
-- Database: `ms_mannschaftsservice`
--

-- --------------------------------------------------------

--
-- Table structure for table `mannschaft`
--

CREATE TABLE `mannschaft` (
  `id` int(33) NOT NULL,
  `Name` varchar(33) DEFAULT NULL,
  `Sportart` varchar(33) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `mannschaft`
--

INSERT INTO `mannschaft` (`id`, `Name`, `Sportart`) VALUES
(1, '1. FC Köln', 'Fussball'),
(2, 'Rhein-Neckar Löwen', 'Handball'),
(3, 'FC Wormersdorf', 'Tennis');

-- --------------------------------------------------------

--
-- Table structure for table `mannschaftmitglied`
--

CREATE TABLE `mannschaftmitglied` (
  `id` int(33) NOT NULL,
  `Mannschaft_ID` int(11) DEFAULT NULL,
  `Mitglied_ID` int(33) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `mannschaftmitglied`
--

INSERT INTO `mannschaftmitglied` (`id`, `Mannschaft_ID`, `Mitglied_ID`) VALUES
(1, 1, 8),
(2, 2, 10),
(3, 3, 9);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `mannschaft`
--
ALTER TABLE `mannschaft`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `mannschaftmitglied`
--
ALTER TABLE `mannschaftmitglied`
  ADD PRIMARY KEY (`id`),
  ADD KEY `Mannschaft_ID` (`Mannschaft_ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `mannschaft`
--
ALTER TABLE `mannschaft`
  MODIFY `id` int(33) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT for table `mannschaftmitglied`
--
ALTER TABLE `mannschaftmitglied`
  MODIFY `id` int(33) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `mannschaftmitglied`
--
ALTER TABLE `mannschaftmitglied`
  ADD CONSTRAINT `mannschaftmitglied_ibfk_1` FOREIGN KEY (`Mannschaft_ID`) REFERENCES `mannschaft` (`id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
