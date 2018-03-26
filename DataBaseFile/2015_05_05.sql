-- --------------------------------------------------------
-- 主机:                           127.0.0.1
-- 服务器版本:                        5.5.5-10.0.15-MariaDB - mariadb.org binary distribution
-- 服务器操作系统:                      Win32
-- HeidiSQL 版本:                  8.3.0.4694
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- 导出 test 的数据库结构
CREATE DATABASE IF NOT EXISTS `test` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `test`;


-- 导出  表 test.bopinfo 结构
CREATE TABLE IF NOT EXISTS `bopinfo` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `bop_type` enum('P','B') NOT NULL DEFAULT 'B' COMMENT '收入或支出',
  `date` date NOT NULL,
  `reason` varchar(300) NOT NULL,
  `balance` double unsigned NOT NULL DEFAULT '0' COMMENT '收入',
  `payment` double unsigned NOT NULL DEFAULT '0' COMMENT '支出',
  `department_id` int(10) unsigned DEFAULT NULL COMMENT '部门',
  `as_id` int(10) unsigned NOT NULL COMMENT '会计主管',
  `cashier_id` int(10) unsigned NOT NULL COMMENT '出纳',
  `handler_id` int(10) unsigned NOT NULL COMMENT '经手人',
  `payee_id` int(10) unsigned DEFAULT NULL COMMENT '领款人',
  `bk_id` int(10) unsigned DEFAULT NULL COMMENT '记账',
  `remark` tinytext COMMENT '备注',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8 COMMENT='收支表';

-- 正在导出表  test.bopinfo 的数据：~9 rows (大约)
DELETE FROM `bopinfo`;
/*!40000 ALTER TABLE `bopinfo` DISABLE KEYS */;
INSERT INTO `bopinfo` (`id`, `bop_type`, `date`, `reason`, `balance`, `payment`, `department_id`, `as_id`, `cashier_id`, `handler_id`, `payee_id`, `bk_id`, `remark`) VALUES
	(1, 'P', '2015-02-09', '买菜', 0, 100, 1, 1, 2, 2, 5, NULL, '支出'),
	(2, 'P', '2015-02-09', '乱花', 0, 200, 2, 1, 3, 3, 4, NULL, '支出'),
	(3, 'B', '2015-02-09', '卖菜kkk官方公告得到', 200, 0, NULL, 1, 4, 4, NULL, 3, '收入'),
	(4, 'B', '2015-02-09', '玩耍爱爱爱', 123, 0, NULL, 1, 5, 5, NULL, 3, '收入'),
	(5, 'P', '2015-02-25', '新插入的', 100, 0, NULL, 1, 4, 5, NULL, 3, '新的收入'),
	(6, 'P', '2015-02-25', '新插入的', 100, 0, NULL, 1, 5, 4, NULL, 3, '新的收入'),
	(7, 'P', '2015-02-25', '呵呵', 200, 0, NULL, 3, 5, 5, NULL, 4, '忽然很感人'),
	(8, 'B', '2015-02-25', '饿不饿', 555, 0, NULL, 1, 3, 5, NULL, 4, '额vev'),
	(9, 'B', '2015-03-17', '呵呵', 2222, 0, NULL, 1, 2, 2, NULL, 4, NULL);
/*!40000 ALTER TABLE `bopinfo` ENABLE KEYS */;


-- 导出  表 test.bs_type 结构
CREATE TABLE IF NOT EXISTS `bs_type` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `type_name` varchar(100) NOT NULL COMMENT '业务类型',
  `procedure_price` text NOT NULL COMMENT '对应的流程与提成',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='业务表';

-- 正在导出表  test.bs_type 的数据：~5 rows (大约)
DELETE FROM `bs_type`;
/*!40000 ALTER TABLE `bs_type` DISABLE KEYS */;
INSERT INTO `bs_type` (`id`, `type_name`, `procedure_price`) VALUES
	(1, '变更核名', '{登记,10}{填表,20}{确认,30}{签名,40}'),
	(2, '租赁合同', '{登记,50}{填表,60}{确认,70}{签名,80}'),
	(3, '酒牌', '{登记,90}{填表,100}{确认,110}{签名,120}'),
	(4, '餐饮证', '{登记,130}{填表,140}{确认,150}{签名,160}'),
	(5, '卫生许可证', '{登记,170}{填表,180}{确认,190}{签名,200}');
/*!40000 ALTER TABLE `bs_type` ENABLE KEYS */;


-- 导出  表 test.departmentinfo 结构
CREATE TABLE IF NOT EXISTS `departmentinfo` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `department_name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8 COMMENT='部门表';

-- 正在导出表  test.departmentinfo 的数据：~5 rows (大约)
DELETE FROM `departmentinfo`;
/*!40000 ALTER TABLE `departmentinfo` DISABLE KEYS */;
INSERT INTO `departmentinfo` (`id`, `department_name`) VALUES
	(1, '财务部'),
	(2, '办证部'),
	(3, '行政部'),
	(4, '人事部'),
	(5, '后勤部');
/*!40000 ALTER TABLE `departmentinfo` ENABLE KEYS */;


-- 导出  表 test.employeeinfo 结构
CREATE TABLE IF NOT EXISTS `employeeinfo` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `employee_name` varchar(50) NOT NULL,
  `employee_sex` enum('女','男') NOT NULL,
  `employee_from` varchar(50) NOT NULL,
  `employee_birthday` date NOT NULL,
  `entry_time` date NOT NULL,
  `basic_salary` int(10) unsigned NOT NULL DEFAULT '0',
  `employee_status` enum('1','0') NOT NULL DEFAULT '1',
  `department_id` int(10) unsigned NOT NULL COMMENT '部门',
  `post_id` int(10) unsigned NOT NULL DEFAULT '1' COMMENT '职务',
  `remark` tinytext,
  PRIMARY KEY (`id`),
  KEY `department_id` (`department_id`),
  KEY `post_id` (`post_id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8 COMMENT='员工表';

-- 正在导出表  test.employeeinfo 的数据：~6 rows (大约)
DELETE FROM `employeeinfo`;
/*!40000 ALTER TABLE `employeeinfo` DISABLE KEYS */;
INSERT INTO `employeeinfo` (`id`, `employee_name`, `employee_sex`, `employee_from`, `employee_birthday`, `entry_time`, `basic_salary`, `employee_status`, `department_id`, `post_id`, `remark`) VALUES
	(1, '小明', '男', '广州', '1990-03-03', '2011-01-02', 3000, '1', 1, 1, NULL),
	(2, '小刚', '男', '广州', '1992-10-23', '2012-03-02', 2000, '1', 2, 1, NULL),
	(3, '小红', '女', '广州', '1991-03-02', '2010-03-02', 1500, '1', 3, 1, NULL),
	(4, '小花', '女', '广州', '1989-06-07', '2014-06-07', 3500, '1', 4, 1, NULL),
	(5, '小王', '男', '广州', '1992-02-03', '2015-03-07', 2500, '1', 5, 1, NULL),
	(6, '呵呵', '女', '不知道的地方方法哥哥', '1989-03-03', '2015-03-14', 3000, '1', 1, 3, NULL);
/*!40000 ALTER TABLE `employeeinfo` ENABLE KEYS */;


-- 导出  表 test.postinfo 结构
CREATE TABLE IF NOT EXISTS `postinfo` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `post_name` varchar(50) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8 COMMENT='职务表';

-- 正在导出表  test.postinfo 的数据：~3 rows (大约)
DELETE FROM `postinfo`;
/*!40000 ALTER TABLE `postinfo` DISABLE KEYS */;
INSERT INTO `postinfo` (`id`, `post_name`) VALUES
	(1, '员工'),
	(2, '总经理'),
	(3, '会计');
/*!40000 ALTER TABLE `postinfo` ENABLE KEYS */;


-- 导出  表 test.salaryinfo 结构
CREATE TABLE IF NOT EXISTS `salaryinfo` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT '编号',
  `year` int(4) unsigned NOT NULL COMMENT '年份',
  `month` enum('1','2','3','4','5','6','7','8','9','10','11','12') NOT NULL COMMENT '月份',
  `employee_id` int(10) unsigned NOT NULL COMMENT '雇员ID',
  `basic_salary` double unsigned NOT NULL DEFAULT '0' COMMENT '基本工资',
  `commission` double unsigned NOT NULL DEFAULT '0' COMMENT '业务提成',
  `meal_supplement` double unsigned NOT NULL DEFAULT '0' COMMENT '餐补',
  `ssb` double unsigned NOT NULL DEFAULT '0' COMMENT '社保(social_security_benefit)',
  `other_benefits` double unsigned NOT NULL DEFAULT '0' COMMENT '其他补贴',
  `bounty` double unsigned NOT NULL DEFAULT '0' COMMENT '奖金',
  `other_deduction` double NOT NULL DEFAULT '0' COMMENT '其他扣减',
  `net_payroll` double unsigned NOT NULL DEFAULT '0' COMMENT '实际工资',
  `remark` tinytext COMMENT '备注',
  PRIMARY KEY (`id`),
  KEY `employee_id` (`employee_id`)
) ENGINE=InnoDB AUTO_INCREMENT=24 DEFAULT CHARSET=utf8 COMMENT='工资表';

-- 正在导出表  test.salaryinfo 的数据：~23 rows (大约)
DELETE FROM `salaryinfo`;
/*!40000 ALTER TABLE `salaryinfo` DISABLE KEYS */;
INSERT INTO `salaryinfo` (`id`, `year`, `month`, `employee_id`, `basic_salary`, `commission`, `meal_supplement`, `ssb`, `other_benefits`, `bounty`, `other_deduction`, `net_payroll`, `remark`) VALUES
	(1, 2015, '1', 1, 3000, 0, 0, 323, 0, 0, 0, 3323, NULL),
	(2, 2015, '3', 1, 3000, 230, 200, 0, 100, 0, 0, 3530, '啊啊啊啊啊'),
	(3, 2015, '3', 2, 2000, 200, 0, 0, 0, 0, 0, 2200, NULL),
	(4, 2015, '3', 3, 1500, 0, 300, 0, 0, 0, 0, 1800, NULL),
	(5, 2015, '3', 4, 3500, 40, 0, 0, 0, 0, 0, 3540, NULL),
	(6, 2015, '3', 5, 2500, 0, 0, 0, 0, 0, 0, 2500, NULL),
	(7, 2015, '2', 1, 3000, 0, 0, 0, 0, 0, 0, 3000, NULL),
	(8, 2015, '2', 2, 2000, 0, 0, 0, 0, 0, 0, 2000, NULL),
	(9, 2015, '2', 3, 1500, 0, 0, 0, 0, 0, 0, 1500, NULL),
	(10, 2015, '2', 4, 3500, 0, 0, 0, 0, 0, 0, 3500, NULL),
	(11, 2015, '2', 5, 2500, 0, 0, 0, 0, 0, 0, 2500, NULL),
	(12, 2015, '4', 1, 3000, 110, 0, 0, 0, 0, 0, 3110, NULL),
	(13, 2015, '4', 2, 2000, 0, 0, 0, 0, 0, 0, 2000, NULL),
	(14, 2015, '4', 3, 2000, 0, 0, 0, 0, 0, 0, 2000, NULL),
	(15, 2015, '4', 4, 3500, 0, 0, 0, 0, 0, 0, 3500, NULL),
	(16, 2015, '4', 5, 2500, 0, 0, 0, 0, 0, 0, 2500, NULL),
	(17, 2015, '4', 6, 3000, 0, 0, 0, 0, 0, 0, 3000, NULL),
	(18, 2015, '5', 1, 3000, 0, 0, 0, 0, 0, 0, 3000, NULL),
	(19, 2015, '5', 2, 2000, 0, 0, 0, 0, 0, 0, 2000, NULL),
	(20, 2015, '5', 3, 1500, 0, 0, 0, 0, 0, 0, 1500, NULL),
	(21, 2015, '5', 4, 3500, 0, 0, 0, 0, 0, 0, 3500, NULL),
	(22, 2015, '5', 5, 2500, 0, 0, 0, 0, 0, 0, 2500, NULL),
	(23, 2015, '5', 6, 3000, 0, 0, 0, 0, 0, 0, 3000, NULL);
/*!40000 ALTER TABLE `salaryinfo` ENABLE KEYS */;


-- 导出  表 test.sheet 结构
CREATE TABLE IF NOT EXISTS `sheet` (
  `id` int(10) unsigned zerofill NOT NULL AUTO_INCREMENT COMMENT '单编号',
  `start_date` date NOT NULL COMMENT '接单日期',
  `finish_date` date DEFAULT NULL COMMENT '完成日期',
  `settle_date` date DEFAULT NULL COMMENT '结单日期',
  `comefrom` varchar(100) DEFAULT NULL COMMENT '接单渠道',
  `accept_id` int(10) unsigned NOT NULL COMMENT '接单人编号',
  `customer_name` varchar(100) NOT NULL COMMENT '客户名称',
  `contacter` varchar(50) NOT NULL COMMENT '联系人',
  `phone_number` varchar(50) NOT NULL COMMENT '联系电话',
  `bs_type_id` int(10) unsigned NOT NULL COMMENT '业务类型编号',
  `price` double unsigned NOT NULL COMMENT '价格',
  `remark` tinytext,
  PRIMARY KEY (`id`),
  KEY `accept_id` (`accept_id`),
  KEY `customer_type_id` (`bs_type_id`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8 COMMENT='接单表';

-- 正在导出表  test.sheet 的数据：~3 rows (大约)
DELETE FROM `sheet`;
/*!40000 ALTER TABLE `sheet` DISABLE KEYS */;
INSERT INTO `sheet` (`id`, `start_date`, `finish_date`, `settle_date`, `comefrom`, `accept_id`, `customer_name`, `contacter`, `phone_number`, `bs_type_id`, `price`, `remark`) VALUES
	(0000000001, '2015-03-15', '2015-03-15', '2015-03-15', '淡淡的', 1, '非官方', '得到', '34444', 1, 333, NULL),
	(0000000006, '2015-03-13', '2015-03-16', '2015-04-11', '问问', 1, '对方当事法官vf', '大幅度', '3443546', 2, 2333, '的'),
	(0000000007, '2015-03-16', '2015-03-16', NULL, '去去去', 2, '搜索', '搜索', '333', 2, 11111, NULL);
/*!40000 ALTER TABLE `sheet` ENABLE KEYS */;


-- 导出  表 test.sheet_bop 结构
CREATE TABLE IF NOT EXISTS `sheet_bop` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `sheet_id` int(11) unsigned NOT NULL COMMENT '单ID',
  `bop_map` text NOT NULL COMMENT '是否可回收，日期，事由，收入，支出',
  PRIMARY KEY (`id`),
  KEY `sheet_id` (`sheet_id`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8 COMMENT='接单相应的收支映射表';

-- 正在导出表  test.sheet_bop 的数据：~9 rows (大约)
DELETE FROM `sheet_bop`;
/*!40000 ALTER TABLE `sheet_bop` DISABLE KEYS */;
INSERT INTO `sheet_bop` (`id`, `sheet_id`, `bop_map`) VALUES
	(1, 1, '{0,2015-01-01,付款,1000,0}{1,2015-01-02,打车,0,20}{1,2015-01-03,用餐,0,30}{0,2015-02-04,玩耍,0,100}'),
	(2, 2, '{0,2015-02-04,预付,800,0}{1,2015-02-04,乱花,0,200}'),
	(3, 12, ''),
	(4, 13, ''),
	(5, 14, ''),
	(6, 15, ''),
	(7, 10, '{1,2015-02-05,www,0,0}'),
	(8, 6, '{0,2015-03-16,得到,0,100}'),
	(9, 7, '');
/*!40000 ALTER TABLE `sheet_bop` ENABLE KEYS */;


-- 导出  表 test.sheet_procedure 结构
CREATE TABLE IF NOT EXISTS `sheet_procedure` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'ID',
  `sheet_id` int(10) unsigned NOT NULL COMMENT '单ID',
  `procedure_map` text NOT NULL COMMENT '相对应的流程项及备注',
  PRIMARY KEY (`id`),
  KEY `sheet_id` (`sheet_id`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8 COMMENT='接单相应的流程映射表';

-- 正在导出表  test.sheet_procedure 的数据：~16 rows (大约)
DELETE FROM `sheet_procedure`;
/*!40000 ALTER TABLE `sheet_procedure` DISABLE KEYS */;
INSERT INTO `sheet_procedure` (`id`, `sheet_id`, `procedure_map`) VALUES
	(1, 1, '{2015-01-10,登记,2,200,}{2015-01-11,填表,1,200,}{2015-01-11,确认,1,30,}{2015-02-01,签名,4,40,}'),
	(2, 2, '{2015-01-14,填表,1,60,真的是}{2015-01-15,确认,2,70,不}{2015-01-16,签名,2,80,容易啊}'),
	(3, 3, '{2015-01-17,登记,3,90,接下来}{2015-01-17,填表,3,100,怎么}{2015-01-17,签名,2,120,搞啊}'),
	(4, 5, '{2015-01-23,登记,4,170,这是}{2015-01-23,填表,4,180,新}{2015-01-24,确认,4,190,添加}{2015-01-25,签名,4,200,的信息}'),
	(5, 6, '{2015-01-20,登记,5,130,}{2015-01-21,填表,5,140,}{2015-01-22,确认,5,150,}'),
	(6, 7, '{2015-02-03,登记,3,90,}{2015-02-03,填表,3,100,}{2015-02-03,签名,3,120,}'),
	(7, 8, '{2015-02-03,登记,2,50,}{2015-02-03,填表,2,60,}'),
	(8, 9, '{2015-02-03,登记,1,130,}{2015-02-03,签名,2,160,}'),
	(9, 10, '{2015-02-03,填表,4,20,}'),
	(10, 11, '{2015-02-03,登记,1,50,}'),
	(11, 12, ''),
	(12, 13, '{2015-02-05,登记,1,90,}'),
	(13, 14, '{2015-02-05,确认,2,150,}'),
	(14, 15, ''),
	(15, 6, '{2015-03-16,登记,1,50,}{2015-03-16,填表,1,60,}'),
	(16, 7, '');
/*!40000 ALTER TABLE `sheet_procedure` ENABLE KEYS */;


-- 导出  表 test.userinfo 结构
CREATE TABLE IF NOT EXISTS `userinfo` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `login_password` varchar(100) NOT NULL,
  `func_password` varchar(100) NOT NULL,
  `level` enum('1','0') NOT NULL DEFAULT '0',
  `status` enum('Y','N') NOT NULL DEFAULT 'N',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8 COMMENT='管理员';

-- 正在导出表  test.userinfo 的数据：~1 rows (大约)
DELETE FROM `userinfo`;
/*!40000 ALTER TABLE `userinfo` DISABLE KEYS */;
INSERT INTO `userinfo` (`id`, `username`, `login_password`, `func_password`, `level`, `status`) VALUES
	(1, 'admin', '67667BA7CBB6D99C3C2F8E42F7D205CC', '685BD9CA8D01B5D5721EF3E9B76D4146', '1', 'Y');
/*!40000 ALTER TABLE `userinfo` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
